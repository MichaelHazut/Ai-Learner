using DataAccessLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class RecommendationDTO
    {
        public string? Topic { get; set; }
        public string? Summary { get; set; }

        public static RecommendationDTO FromRecommendation(Recommendation Recommendation)
        {
            return new RecommendationDTO
            {
                Topic = Recommendation.Topic,
                Summary = Recommendation.Summary
            };
        }
    }
}
