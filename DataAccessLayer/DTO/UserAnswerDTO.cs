using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class UserAnswerDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public DateTime AnswerDate { get; set; }
        public string? UserId { get; set; }

        public static UserAnswerDTO FromUserAnswer(UserAnswer userAnswer)
        {
            return new UserAnswerDTO
            {
                Id = userAnswer.UserAnswerId,
                QuestionId = userAnswer.QuestionId,
                AnswerId = userAnswer.AnswerId,
                AnswerDate = userAnswer.AnswerDate,
                UserId = userAnswer.UserId,
            };
        }
    }
}
