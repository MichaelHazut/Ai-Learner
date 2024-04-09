using DataAccessLayer.dbContext;
using DataAccessLayer.DTO;
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

        public async Task<List<UserAnswer>> CreateUsersAnswers(List<UserAnswerDTO> userAnswersDTOs)
        {
            List<UserAnswer> userAnswers = userAnswersDTOs.Select(ua => new UserAnswer
            {
                QuestionId = ua.QuestionId,
                AnswerId = ua.AnswerId,
                AnswerDate = ua.AnswerDate,
                UserId = ua.UserId
            }).ToList();

            await _context.AddRangeAsync(userAnswers);
            return userAnswers;
        }
        public async Task UpdateUserAnswers(List<UserAnswerDTO> userAnswersDTOs)
        {
            foreach (var dto in userAnswersDTOs)
            {
                // Retrieve the corresponding UserAnswer entity from the database based on the QuestionId
                var userAnswer = await _context.Set<UserAnswer>()
                    .FirstOrDefaultAsync(ua => ua.QuestionId == dto.QuestionId);

                if (userAnswer != null)
                {
                    // Update the UserAnswer entity with the values from the DTO
                    userAnswer.AnswerId = dto.AnswerId;
                    userAnswer.AnswerDate = dto.AnswerDate;
                    // Update other properties as needed

                    // Mark the UserAnswer entity as modified
                    Update(userAnswer);
                }

            }
        }
    }
}
