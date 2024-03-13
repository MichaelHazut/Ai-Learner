using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int MaterialId { get; set; }
        public string? Text { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual Material? Material { get; set; }
        public virtual ICollection<Answer>? Answers { get; set; }
    }

}
