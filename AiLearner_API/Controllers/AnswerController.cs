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
    public class AnswerController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetAnswers(int materialId)
        {
            List<Question> questions = await _unitOfWork.Questions.GetQuestions(materialId);
            if (questions is null || questions.Count == 0)
                return NotFound("No questions found for material id: {" + materialId + "}");


            List<Answer> answers = questions.SelectMany(question => _unitOfWork.Answers.GetAnswers(question.QuestionId)).ToList();

            if (answers is null || answers.Count == 0)
                return NotFound("No answers found");

            List<AnswerDTO> answerDTOs = answers.Select(AnswerDTO.FromAnswer).ToList();


            return Ok(answerDTOs);

        }
    }
}
