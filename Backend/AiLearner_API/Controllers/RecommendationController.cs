using AiLearner_API.Services;
using DataAccessLayer.DTO;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecommendationController(IUnitOfWork unitOfWork, CachingService cachingService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly CachingService _cachingService = cachingService;

        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetRecommendations(int materialId)
        {
            bool isCached = _cachingService.TryGetCachedItem(materialId.ToString(), out List<RecommendationDTO>? RecommendationDTOs);
            if (isCached is true)
                return Ok(RecommendationDTOs);

            List<Recommendation> recommendations = await _unitOfWork.Recommendations.GetRecommendations(materialId);
            if (recommendations is null || recommendations.Count == 0)
                return NotFound("No recommendations found for material id: {" + materialId + "}");

            RecommendationDTOs = recommendations.Select(RecommendationDTO.FromRecommendation).ToList();
            _cachingService.CacheItem(materialId.ToString(), RecommendationDTOs);

            return Ok(RecommendationDTOs);
        }
    }
}
