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
    public class UserAnswerController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetUserAnswers(int materialId)
        {
            List<UserAnswer> userAnswers = await _unitOfWork.UserAnswers.GetUserAnswers(materialId);
            if (userAnswers is null || userAnswers.Count == 0)
                return NotFound("No user answers found for Material id: {" + materialId + "}");

            List<UserAnswerDTO> userAnswerDTOs = userAnswers.Select(UserAnswerDTO.FromUserAnswer).ToList();

            return Ok(userAnswerDTOs);
        }

        
    }
}
