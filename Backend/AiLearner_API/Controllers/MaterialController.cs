using AiLearner_API.Services;
using AiLearner_ClassLibrary.OpenAi_Service;
using DataAccessLayer.DTO;
using DataAccessLayer.Models;
using DataAccessLayer.Models.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace AiLearner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MaterialController(IUnitOfWork unitOfWork, OpenAIService openAIService, CachingService cachingService, JwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly OpenAIService _openAIService = openAIService;
        private readonly CachingService _cachingService = cachingService;
        private readonly JwtTokenService _jwtTokenService = jwtTokenService;

        [HttpGet]
        public async Task<IActionResult> GetMaterials()
        {
            var principal = _jwtTokenService.ValidateToken(Request.Cookies["AccessToken"]!);
            var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                                ?? throw new SecurityTokenException("User ID is missing in the token.");


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

        [HttpGet("material/{materialId:int}")]
        public async Task<IActionResult> GetMaterial(int materialId)
        {
            bool isCached = _cachingService.TryGetCachedItem(materialId.ToString(), out Material? material);
            if (isCached is false)
            {
                material = await _unitOfWork.Materials.GetByIdAsync(materialId);
                if (material == null)
                    return NotFound("Material Not Found");

                _cachingService.CacheItem(materialId.ToString(), material);
            }
            MaterialDTO materialDTO = MaterialDTO.FromMaterial(material!);
            return Ok(materialDTO);

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

            // Create the material with questions and answers in the database
            bool isSuccess = await _unitOfWork.CreateMaterialWithQuestionsAndAnswers(requestDto.UserId, material);

            // Clear the cached items related to the user
            _cachingService.RemoveCachedItem<List<Material>>(requestDto.UserId);

            // Return the created material or a Not found response
            return isSuccess ? new CreatedResult("/material", material) : NotFound();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var principal = _jwtTokenService.ValidateToken(Request.Cookies["AccessToken"]!);
            var userId = (principal.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                                ?? throw new SecurityTokenException("User ID is missing in the token.");

            // Clear the cached items related to the material
            _cachingService.RemoveCachedItem<Material>(id.ToString());
            _cachingService.RemoveCachedItem<List<Material>>(userId);
            _cachingService.RemoveCachedItem<List<Question>>(id.ToString());
            _cachingService.RemoveCachedItem<List<Answer>>(id.ToString());

            // Try to delete the material from the database and return a bool response
            bool isDeleted = await _unitOfWork.DeleteMaterialAsync(id);
            if (isDeleted is false)
                return NotFound("Material Not Found");


            return Ok("Material Deleted Successfully");
        }
    }
}
