using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class UserAnswer
    {
        public int UserAnswerId { get; set; }
        public int QuestionId { get; set; }
        public string? UserId { get; set; }
        public int AnswerId { get; set; }
        public DateTime AnswerDate { get; set; }

        public virtual Question? Question { get; set; }
        public virtual User? User { get; set; }
        public virtual Answer? Answer { get; set; }
    }

}
