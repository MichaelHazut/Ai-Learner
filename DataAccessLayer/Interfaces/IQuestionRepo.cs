using DataAccessLayer.Models.Entities;
using DataAccessLayer.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IQuestionRepo : IEntityDataAccess<Question>
    {
        Task<List<Question>> CreateQuestion(int materialId, List<Questions> questions);
    }
}
