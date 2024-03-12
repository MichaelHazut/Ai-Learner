using DataAccessLayer.DTO;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class QuestionController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetQuestions(int materialId)
        {
            var questions = await _unitOfWork.Questions.GetQuestions(materialId);

            if (questions is null || questions.Count == 0)
                return NotFound("No questions found");

            List<QuestionDTO> questionDTOs = questions.Select(QuestionDTO.FromQuestion).ToList();
            return Ok(questionDTOs);
        }


        [HttpDelete("id")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _unitOfWork.Questions.GetByIdAsync(id);
            
            if (question is null)
                return NotFound("Question not found");


            _unitOfWork.Questions.Delete(question);
            int changes = await _unitOfWork.CompleteAsync();

            if (changes <= 0)
                return BadRequest("Question deletion failed");


            return Ok("Question deleted successfully");
        }


    }
}
