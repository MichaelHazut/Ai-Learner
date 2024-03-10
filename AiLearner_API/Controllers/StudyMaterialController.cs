using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AiLearner_API.Controllers
{
    public class StudyMaterialController(IUnitOfWork unitOfWork, OpenAIService aIService) : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork = unitOfWork;
        private readonly OpenAIService OpenAIService = aIService;


        [HttpPost("materials")]
        public async Task<IActionResult> CreateMaterials([FromBody] MaterialRequestDto requestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            StudyMaterial? material = null;
            bool isValid = false;

            // Try to generate a valid study material 10 times
            for (int attempt = 0; attempt < 10 && !isValid; attempt++)
            {
                // Call the OpenAI service to generate a study material
                var res = await OpenAIService.CallChatGPTAsync(requestDto.Content, requestDto.NumOfQuestions);
                if (res == null) continue;

                // Clean the JSON response and deserialize it into a StudyMaterial object
                string cleanJson = JsonService.CleanJson(res);
                material = JsonService.DeserializeJson<StudyMaterial>(cleanJson);
                if (material == null) continue;

                // Set the content of the study material and validate it
                material.Content = requestDto.Content;
                isValid = material.ValidateStudyMaterial();
            }

            // if the loop fails to generate a valid study material, return a bad request
            if (!isValid || material == null) return BadRequest("Unable to generate valid study material.");

            bool isSuccess = await UnitOfWork.CreateMaterialWithQuestionsAndAnswers(requestDto.UserId, material);

            return isSuccess ? Ok("Created material") : NotFound();
        }


        [HttpDelete("materials")]
        public async Task<IActionResult> DeleteMaterial([FromBody] int materialId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isDeleted = await UnitOfWork.DeleteMaterialAsync(materialId);

            return isDeleted ? Ok("Material Deleted Successfully") : NotFound("Material Not Found");
        }
    }
}
