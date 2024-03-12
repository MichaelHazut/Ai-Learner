using DataAccessLayer.Models.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUsersAnswersRepo : IEntityDataAccess<UserAnswer>
    {
        Task<List<UserAnswer>> GetUserAnswers(int materialId);
        Task<UserAnswer> CreateUsersAnswers(string userId, int questionId, int answerId);
    }
}
