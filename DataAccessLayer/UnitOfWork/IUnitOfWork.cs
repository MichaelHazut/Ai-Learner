using DataAccessLayer.Interfaces;
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
        Task<int> CompleteAsync();
    }
}
