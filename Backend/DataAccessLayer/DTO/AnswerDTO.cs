using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public string? Text { get; set; }

        public static AnswerDTO FromAnswer(Answer answer)
        {
            return new AnswerDTO
            {
                Id = answer.AnswerId,
                QuestionId = answer.QuestionId,
                IsCorrect = answer.IsCorrect,
                Text = answer.Text
            };
        }
    }
}
