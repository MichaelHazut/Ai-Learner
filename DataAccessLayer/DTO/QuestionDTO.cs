using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Text { get; set; }

        public static QuestionDTO FromQuestion(Question question)
        {
            return new QuestionDTO
            {
                Id = question.QuestionId,
                MaterialId = question.MaterialId,
                CreateDate = question.CreateDate,
                Text = question.Text,
            };
        }
    }
}
