using DataAccessLayer.dbContext;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RecommendationRepo(AiLearnerDbContext context) : RepositoryBase<Recommendation>(context), IRecommendationRepo
    {
        public async Task<List<Recommendation>> GetRecommendations(int materialId)
        {
            return await _context.Set<Recommendation>().Where(r => r.MaterialId == materialId).ToListAsync();
        }

        public async Task<List<Recommendation>> CreateRecommendation(int materialId, List<Recommendation> recommendations)
        {
            List<Recommendation> recommendationEntities = [];

            //Interate through the recommendations and create the recommendation's entities
            foreach (var recommendation in recommendations)
            {
                recommendationEntities.Add(new Recommendation
                {
                    MaterialId = materialId,
                    Topic = recommendation.Topic,
                    Summery = recommendation.Summery
                });

            }
            await _context.AddRangeAsync(recommendationEntities);

            return recommendationEntities;
        }
    }
    {
    }
}
