using DataAccessLayer.Models.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IRecommendationRepo
    {
        Task<List<Recommendation>> GetRecommendations(int materialId);
        Task<List<Recommendation>> CreateRecommendation(int materialId, List<Recommendation> recommendations);
    }
}