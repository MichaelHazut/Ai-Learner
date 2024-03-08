using DataAccessLayer.Models.Entities;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IQuestionRepo : IEntityDataAccess<Question>
    {
        Task<List<Question>> CreateQuestion(int materialId, List<Questions> questions);
    }
}
