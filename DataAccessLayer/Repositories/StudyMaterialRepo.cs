using AiLearner_ClassLibrary.OpenAi_Service;

using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public class StudyMaterialRepo(OpenAIService openAIService)
    {
        private readonly OpenAIService _openAIService = openAIService;

        //Use to get a new StudyMaterial from the GPT API
        public async Task<StudyMaterial> NewMaterial(string content, int numOfQuestions)
        {
            //Get Json response from GPT API
            string response = await _openAIService.CallChatGPTAsync(content, numOfQuestions);
            string cleanJson = JsonService.CleanJson(response);
            
            //Deserialize Json to StudyMaterial object and check if it is null
            var material = JsonService.DeserializeJson<StudyMaterial>(cleanJson)
                ?? throw new Exception("Failed to create material");

            //validate the StudyMaterial
            bool isValid = material.ValidateStudyMaterial();
            if (!isValid) throw new Exception("Invalid Study Material");

            material.Content = content;
            return material;
        }
    }
}
