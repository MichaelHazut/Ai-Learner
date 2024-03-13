using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repositories
{
    public class UsersAnswersRepo(AiLearnerDbContext context) : RepositoryBase<UserAnswer>(context), IUsersAnswersRepo
    {
        public async Task<List<UserAnswer>> GetUserAnswers(int materialId)
        {
            var questions = _context.Set<Question>().Where(q => q.MaterialId == materialId);
            var userAnswers = await _context.Set<UserAnswer>().Where(ua => questions.Contains(ua.Question)).ToListAsync();
            return userAnswers;
        }

        public async Task<UserAnswer> CreateUsersAnswers(string userId, int questionId, int answerId)
        {
            UserAnswer userAnswer = new()
            {
                UserId = userId,
                QuestionId = questionId,
                AnswerId = answerId,
                AnswerDate = DateTime.Now
            };

            await _context.AddAsync(userAnswer);
            await _context.SaveChangesAsync();
            return userAnswer;
        }
    }
}
