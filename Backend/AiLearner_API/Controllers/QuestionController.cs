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
    public class QuestionController(IUnitOfWork unitOfWork, CachingService cachingService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly CachingService _cachingService = cachingService;


        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetQuestions(int materialId)
        {
            // Try to get the cached item and assign it to the questionsDTOs variable
            bool isCached = _cachingService.TryGetCachedItem(materialId.ToString(), out List<QuestionDTO>? questionsDTOs);
            if (isCached is true)
                return Ok(questionsDTOs);

            // Attempt to get the questions from the database
            List<Question>? questions = await _unitOfWork.Questions.GetQuestions(materialId);
            if (questions is null || questions.Count == 0)
                return NotFound("No questions found");

            // Convert the questions to QuestionDTOs and cache them
            List<QuestionDTO> questionDTOs = questions.Select(QuestionDTO.FromQuestion).ToList();
            _cachingService.CacheItem(materialId.ToString(), questionDTOs);
            
            return Ok(questionDTOs);
        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            // Attempt to get the question from the database
            var question = await _unitOfWork.Questions.GetByIdAsync(id);

            if (question is null)
                return NotFound("Question not found");

            // Attempt to delete the question from the database
            _unitOfWork.Questions.Delete(question);
            int changes = await _unitOfWork.CompleteAsync();

            // If the deletion failed, return a bad request
            if (changes <= 0)
                return BadRequest("Question deletion failed");

            // Remove the question from the cache
            _cachingService.RemoveCachedItem<QuestionDTO>(question.MaterialId.ToString());

            return Ok("Question deleted successfully");
        }


    }
}
