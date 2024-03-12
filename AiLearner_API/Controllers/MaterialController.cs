using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialController(IUnitOfWork unitOfWork, OpenAIService aIService) : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork = unitOfWork;
        private readonly OpenAIService OpenAIService = aIService;


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMaterials(string userId)
        {
            List<Material> materials = await UnitOfWork.GetMaterials(userId);

            if (materials == null || materials.Count == 0) 
                return NotFound("No Materials Found");

            return Ok(materials);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] MaterialRequestDto requestDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            StudyMaterial? material = null;
            bool isValid = false;

            // Try to generate a valid study material 10 times
            for (int attempt = 0; attempt < 10 && !isValid; attempt++)
            {
                // Call the OpenAI service to generate a study material
                var res = await OpenAIService.CallChatGPTAsync(requestDto.Content, requestDto.NumOfQuestions);
                if (res == null) 
                    continue;

                // Clean the JSON response and deserialize it into a StudyMaterial object
                string cleanJson = JsonService.CleanJson(res);
                material = JsonService.DeserializeJson<StudyMaterial>(cleanJson);
                if (material == null) 
                    continue;

                // Set the content of the study material and validate it
                material.Content = requestDto.Content;
                isValid = material.ValidateStudyMaterial();
            }

            // if the loop fails to generate a valid study material, return a bad request
            if (!isValid || material == null) return BadRequest("Unable to generate valid study material.");

            bool isSuccess = await UnitOfWork.CreateMaterialWithQuestionsAndAnswers(requestDto.UserId, material);

            return isSuccess ? Ok("Created material") : NotFound();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isDeleted = await UnitOfWork.DeleteMaterialAsync(id);

            return isDeleted ? Ok("Material Deleted Successfully") : NotFound("Material Not Found");
        }
    }
}
