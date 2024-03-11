using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IUserRepo Users { get; }
        IMaterialRepo Materials { get; }
        IQuestionRepo Questions { get; }
        IAnswerRepo Answers { get; }
        IUsersAnswersRepo UserAnswers { get; }
        IRefreshTokenRepo RefreshToken { get; }


        Task<int> CompleteAsync();
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> CreateMaterialWithQuestionsAndAnswers(string userId, StudyMaterial material);
        Task<List<Material>> GetMaterials(string userId);
        Task<bool> DeleteMaterialAsync(int materialId);
        Task<bool> DeleteQuestionAsync(int questionId);
    }
}
