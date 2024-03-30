using AiLearner_API.Services;
using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;


namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MaterialController(IUnitOfWork unitOfWork, OpenAIService openAIService, CachingService cachingService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly OpenAIService _openAIService = openAIService;
        private readonly CachingService _cachingService = cachingService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMaterials(string userId)
        {
            // Try to get the cached item and assign it to the materials variable
            bool isCached = _cachingService.TryGetCachedItem(userId, out List<Material>? materials);

            // If the item is not cached, get the materials from the database
            if (isCached is false)
            {
                // Get the materials from the database
                materials = await _unitOfWork.Materials.GetMaterials(userId);
                if (materials == null || materials.Count == 0)
                    return NotFound("No Materials Found");

                // Cache the materials
                _cachingService.CacheItem(userId, materials);
            }

            // Convert the materials to MaterialDTOs and return them
            List<MaterialDTO> materialDTOs = materials!.Select(MaterialDTO.FromMaterial).ToList();
            return Ok(materialDTOs);
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
                var res = await _openAIService.CallChatGPTAsync(requestDto.Content, requestDto.NumOfQuestions);
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

            bool isSuccess = await _unitOfWork.CreateMaterialWithQuestionsAndAnswers(requestDto.UserId, material);
            
            return isSuccess ? new CreatedResult("/material", material) : NotFound();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Try to delete the material from the database and return a bool response
            bool isDeleted = await _unitOfWork.DeleteMaterialAsync(id);
            if (isDeleted is false) 
                return NotFound("Material Not Found");

            // Clear the cached items related to the material
            _cachingService.RemoveCachedItem<List<Material>>(id.ToString());
            _cachingService.RemoveCachedItem<List<Question>>(id.ToString());
            _cachingService.RemoveCachedItem<List<Answer>>(id.ToString());

            return Ok("Material Deleted Successfully");
        }
    }
}
