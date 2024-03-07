using DataAccessLayer.dbContext;
using DataAccessLayer.models;
using DataAccessLayer.models.Entities;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class QuestionRepo(AiLearnerDbContext context) : RepositoryBase<Question>(context)
    {
        public async Task<List<Question>> CreateQuestion(int materialId, List<Questions> questions)
        {
            List<Question> questionEntities = new List<Question>();
            List<Answer> answerEntities = new List<Answer>();

            //Interate through the questions and create the question and answer entities

            foreach (var question in questions)
            {
                questionEntities.Add(new Question
                {
                    MaterialId = materialId,
                    Text = question.Question,
                    CreateDate = DateTime.Now
                });

                //itterate through the questions answers and create entities
                //foreach (var option in question.Options!)
                //{
                //    questionEntities.Add(new Question
                //    {
                //        MaterialId = materialId,
                //        Text = option.Value,
                //        CreateDate = DateTime.Now
                //    });

                //    answerEntities.Add(new Answer
                //    {
                //        Text = option.Value,
                //        IsCorrect = option.Key == question.Answer
                //    });
                //}
            }
            await _context.AddRangeAsync(questionEntities);
            await _context.SaveChangesAsync();

            //Assign the questionId to the answer
            //answerEntities.Where(answer =>
            //    answerEntities
            //    .Any(a => a.Text == answer.Text))
            //    .ToList()
            //    .ForEach(answer => answer.QuestionId = questionEntities
            //    .First(q => q.Text == answer.Text).QuestionId);

            //await _context.AddRangeAsync(answerEntities);
            //await _context.SaveChangesAsync();
            return questionEntities;
        }
    }
}
