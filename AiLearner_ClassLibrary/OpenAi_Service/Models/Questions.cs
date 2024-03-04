using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLearner_ClassLibrary.OpenAi_Service.Models
{
    public class Questions
    {
        public string ? Question { get; set; }
        public Dictionary<string, string>? Options { get; set; }
        public string? Answer { get; set; }
    }
}
