using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Models;


namespace AiLearner_ClassLibrary.OpenAi_Service
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public OpenAIService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> CallChatGPTAsync(string message, int numberOfQuestion)
        {
            OpenAIAPI openAIAPI = new(_apiKey);
            var chat = openAIAPI.Chat.CreateConversation();

            chat.Model = new Model("gpt-3.5-turbo-0125");

            chat.AppendSystemMessage("you generate multiple answer questions base on text. 4 answers per question, make some of the 2 answers.");
            chat.AppendSystemMessage("Youre only response will be a JSON object with six variables:topic string, summary 1-3 sentences describing the text, a a questions object that contains: question string, options object with key value pair, answer a letter char");

            chat.AppendUserInput(message + $"\n generate {numberOfQuestion} question base on this study material");

            return await chat.GetResponseFromChatbotAsync();
        }
    }
}
