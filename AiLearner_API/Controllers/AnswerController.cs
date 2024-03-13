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
    public class AnswerController(IUnitOfWork unitOfWork, CachingService cachingService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly CachingService _cachingService = cachingService;

        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetAnswers(int materialId)
        {
            // Try to get the cached item and assign it to the answerDTOs variable
            bool isCached = _cachingService.TryGetCachedItem(materialId.ToString(), out List<AnswerDTO>? answerDTOs);
            if (isCached is true)
                return Ok(answerDTOs);

            // Attempt to get the questions from the database
            List<Question> questions = await _unitOfWork.Questions.GetQuestions(materialId);
            if (questions is null || questions.Count == 0)
                return NotFound("No questions found for material id: {" + materialId + "}");

            // Attempt to get the answers from the database
            List<Answer> answers = questions.SelectMany(question => _unitOfWork.Answers.GetAnswers(question.QuestionId)).ToList();
            if (answers is null || answers.Count == 0)
                return NotFound("No answers found");

            // Convert the answers to AnswerDTOs and cache them
            answerDTOs = answers.Select(AnswerDTO.FromAnswer).ToList();
            _cachingService.CacheItem(materialId.ToString(), answerDTOs);

            return Ok(answerDTOs);

        }
    }
}
