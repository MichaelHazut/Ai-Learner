using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models.Entities
{
    public class Recommendation
    {
        public int RecommendationId { get; set; }
        public int MaterialId { get; set; } 
        public string? Topic { get; set; }
        public string? Summary { get; set; }

        public virtual Material? Material { get; set; }
    }
}
