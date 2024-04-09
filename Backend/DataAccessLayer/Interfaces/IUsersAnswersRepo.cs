using DataAccessLayer.DTO;
using DataAccessLayer.Models.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUsersAnswersRepo : IEntityDataAccess<UserAnswer>
    {
        Task<List<UserAnswer>> GetUserAnswers(int materialId);
        Task<List<UserAnswer>> CreateUsersAnswers(List<UserAnswerDTO> userAnswersDTOs);
        Task UpdateUserAnswers(List<UserAnswerDTO> userAnswersDTOs);
    }
}
