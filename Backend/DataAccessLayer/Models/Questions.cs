using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    //Questions class is not an entity
    //It is used to create questions and answers
    public class Questions
    {
        public string? Question { get; set; }
        public Dictionary<string, string>? Options { get; set; }
        public string? Answer { get; set; }
    }
}
