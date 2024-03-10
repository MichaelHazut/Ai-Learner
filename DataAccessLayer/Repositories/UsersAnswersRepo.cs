using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;


namespace DataAccessLayer.Repositories
{
    public class UsersAnswersRepo(AiLearnerDbContext context) : RepositoryBase<UserAnswer>(context), IUsersAnswersRepo
    {
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
