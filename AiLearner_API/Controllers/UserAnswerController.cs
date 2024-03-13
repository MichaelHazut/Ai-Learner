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
    //[Authorize]
    public class UserAnswerController(IUnitOfWork unitOfWork, CachingService cachingService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly CachingService _cachingService = cachingService;


        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetUserAnswers(int materialId)
        {
            // Try to get the cached item and assign it to the userAnswerDTOs variable
            bool isCached = _cachingService.TryGetCachedItem(materialId.ToString(), out List<UserAnswerDTO>? userAnswerDTOs);
            if (isCached is true)
                return Ok(userAnswerDTOs);

            // Attempt to get the user answers from the database
            List<UserAnswer> userAnswers = await _unitOfWork.UserAnswers.GetUserAnswers(materialId);
            if (userAnswers is null || userAnswers.Count == 0)
                return NotFound("No user answers found for Material id: {" + materialId + "}");

            // Convert the UserAnswers to UserAnswerDTOs and cache them
            userAnswerDTOs = userAnswers.Select(UserAnswerDTO.FromUserAnswer).ToList();
            _cachingService.CacheItem(materialId.ToString(), userAnswerDTOs);

            return Ok(userAnswerDTOs);
        }

        
    }
}
