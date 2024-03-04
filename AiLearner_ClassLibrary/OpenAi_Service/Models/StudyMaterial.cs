using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLearner_ClassLibrary.OpenAi_Service.Models
{
    public class StudyMaterial
    {
        public string? Topic { get; set; }
        public string? Summary { get; set; }
        public List<Questions>? Questions { get; set; }
    }



}
