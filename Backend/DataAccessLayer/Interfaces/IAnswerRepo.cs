using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAnswerRepo : IEntityDataAccess<Answer>
    {
        IQueryable<Answer> GetAnswers(int questionId);
        Task<List<Answer>> CreateAnswer(List<Question> idList, List<Questions> textList);
    }
}
