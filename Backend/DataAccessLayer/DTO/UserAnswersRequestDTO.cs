using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class UserAnswersRequestDTO
    {
        public required List<UserAnswerDTO> UserAnswers { get; set; }
        public required string UserId { get; set; }
    }
}
