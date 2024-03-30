using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class MaterialRequestDto
    {
        public required string UserId { get; set; }
        public required string Content { get; set; }
        public int NumOfQuestions { get; set; } = 10;
    }
}
