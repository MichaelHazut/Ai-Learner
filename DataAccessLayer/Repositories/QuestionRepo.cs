using DataAccessLayer.dbContext;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.Models;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    //This class is responsible for creating a question entity
    public class QuestionRepo(AiLearnerDbContext context) : RepositoryBase<Question>(context), IQuestionRepo
    {
        public async Task<List<Question>> GetQuestions(int materialId)
        {
            return await _context.Set<Question>().Where(q => q.MaterialId == materialId).ToListAsync();
        }


        public async Task<List<Question>> CreateQuestion(int materialId, List<Questions> questions)
        {
            List<Question> questionEntities = [];

            //Interate through the questions and create the question's entities
            foreach (var question in questions)
            {
                questionEntities.Add(new Question
                {
                    MaterialId = materialId,
                    Text = question.Question,
                    CreateDate = DateTime.Now
                });

            }
            await _context.AddRangeAsync(questionEntities);

            return questionEntities;
        }
    }
}
