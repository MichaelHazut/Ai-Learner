using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.UnitOfWork
{
    // The UnitOfWork class is responsible for managing the repositories and the database context.
    // It implements the IUnitOfWork interface and provides the implementation for the repositories.
    public class UnitOfWork(AiLearnerDbContext context, IUserRepo userRepo, IMaterialRepo materialRepo, IQuestionRepo questionRepo, IAnswerRepo answerRepo, IUsersAnswersRepo usersAnswersRepo) : IUnitOfWork
    {
        // The UnitOfWork class has a constructor that takes the database context and the repositories as parameters.
        public IUserRepo Users { get; private set; } = userRepo;
        public IMaterialRepo Materials { get; private set; } = materialRepo;
        public IQuestionRepo Questions { get; private set; } = questionRepo;
        public IAnswerRepo Answers { get; private set; } = answerRepo;
        public IUsersAnswersRepo UserAnswers { get; private set; } = usersAnswersRepo;

        private readonly AiLearnerDbContext Context = context;

        
        public async Task<bool> CreateMaterialWithQuestionsAndAnswers(string userId, StudyMaterial studyMaterial)
        {
            //Create the material entities and save it to the database
            Material material = await Materials.CreateMaterial(userId, studyMaterial.Topic, studyMaterial.Content, studyMaterial.Summary);
            await CompleteAsync();

            //Create the question entities and save it to the database
            List<Question> questions = await Questions.CreateQuestion(material.MaterialId, studyMaterial.Questions);
            await CompleteAsync();

            //Create the answer entities and save it to the database
            await Answers.CreateAnswer(questions, studyMaterial.Questions);

            //save the changes and return true if the number of changes are greater than 0
            int changes = await CompleteAsync();
            return changes > 0;
        }


        // The DeleteUserAsync method deletes a user and all the materials associated with the user.
        public async Task<bool> DeleteUserAsync(string userId)
        {
            //Get the user by id and check if it exists
            var user = await Users.GetByIdAsync(userId);
            if (user == null) return false;

            //Get the materials by the user id
            List<Material> materials = await Context.Materials.Where(material => material.UserId == userId).ToListAsync();

            //if the user has materials, iterate through the materials and delete the material
            if (materials.Count > 0)
            {
                foreach (var item in materials)
                {
                    bool isSuccsessful = await DeleteMaterialAsync(item.MaterialId);
                    if (isSuccsessful == false) return false;
                }
            };
            //remove the user
            Users.Delete(user);

            //save the changes and return true if the number of changes are greater than 0
            int changes = await CompleteAsync();
            return changes > 0;
        }


        public async Task<bool> DeleteMaterialAsync(int materialId)
        {
            //Get the material by id
            var material = await Materials.GetByIdAsync(materialId);
            if (material == null) return false;

            //Get the questions by the material id
            List<Question> questions = await Context.Questions.Where(question => question.MaterialId == materialId).ToListAsync();
            if (questions.Count == 0) return false;
            
            //iterate through the questions and delete the question and answers
            foreach (var question in questions)
            {
                bool isSuccsessful = await DeleteQuestionAsync(question.QuestionId);
                if (isSuccsessful == false) return false;
            }
            //remove the material
            Context.Materials.Remove(material);

            //save the changes and return true
            //if the the number of changes are greater than 0
            int changes = await CompleteAsync();
            return changes > 0;
        }


        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            //Get the question by id and check if it exists
            Question? question = await Context.Questions.FindAsync(questionId);
            if (question is null) return false;

            //Get the Answers by id and check if it exists
            List<Answer> answers = await Context.Answers.Where(answer => answer.QuestionId == questionId).ToListAsync();
            if (answers.Count == 0) return false;

            //delete the user users answers along with the answers and the question
            if(DeleteUserAnswers(questionId) == false) return false;
            Context.Answers.RemoveRange(answers);
            Context.Questions.Remove(question);
            return true;
        }


        public bool DeleteUserAnswers(int questionId)
        {
            var userAnswers = Context.UserAnswers.Where(answer => answer.QuestionId == questionId).ToList();
            Context.UserAnswers.RemoveRange(userAnswers);
            return true;
        }



        // The CompleteAsync method saves the changes to the database
        // and returns the number of state enteries wirrten to the database.
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException(e.Message);
            }
        }

        // The DisposeAsync method disposes the database context.
        public async ValueTask DisposeAsync()
        {
            if (Context != null)
            {
                await Context.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
        public void Dispose()
        {
            var disposeTask = DisposeAsync();

            if (disposeTask.IsCompletedSuccessfully)
            {
                disposeTask.GetAwaiter().GetResult();
            }

            GC.SuppressFinalize(this);
        }
    }

}

