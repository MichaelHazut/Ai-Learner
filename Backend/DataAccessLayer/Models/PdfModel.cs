using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PdfModel
    {
        public required string Header { get; set; }
        public required string Content { get; set; }
        public required List<Question> Questions { get; set; }
        public required List<Answer> Answers { get; set; }
    }
}
