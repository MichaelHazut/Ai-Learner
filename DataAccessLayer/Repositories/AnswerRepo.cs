using DataAccessLayer.dbContext;
using DataAccessLayer.models;
using DataAccessLayer.models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AnswerRepo(AiLearnerDbContext context) : RepositoryBase<Answer>(context)
    {
        public async Task<List<Answer>> CreateAnswer(List<Question> idList, List<Questions> textList)
        {
            List<Answer> answerEntities = new List<Answer>();
            foreach (var question in idList)
            {
                foreach (var text in textList)
                {
                    if (question.Text == text.Question)
                    {
                        foreach (var answer in text.Options!)
                        {
                            answerEntities.Add(new Answer
                            {
                                QuestionId = question.QuestionId,
                                Text = answer.Value,
                                IsCorrect = answer.Key == text.Answer
                            });
                        }

                    }
                }
            }
            await _context.AddRangeAsync(answerEntities);
            await _context.SaveChangesAsync();
            return answerEntities;
        }
    }
}
