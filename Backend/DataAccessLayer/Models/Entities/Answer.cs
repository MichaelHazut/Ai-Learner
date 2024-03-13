using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int ?QuestionId { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Question? Question { get; set; }
    }

}
