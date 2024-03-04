//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using OpenAI_API;
//using OpenAI_API.Models;
//using System;
//using System.Text;

//namespace AiLearner_API.Services
//{
//    public class OpenAIService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string? _apiKey;

//        public OpenAIService(IConfiguration configuration)
//        {
//            _httpClient = new HttpClient();
//            _apiKey = configuration["OpenAI:ApiKey"];
//            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
//        }

//        public async Task<string> CallChatGPTAsync(string message)
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(
//                new
//                {
//                    model = "gpt-3.5-turbo",
//                    messages = new[] {
//                        new { role = "system", content = "you generate multiple answer questions base on text in a clean JSON format with three variables: question string, options object with key value pair, answer a letter char." },
//                        new { role = "user", content = $"generate 5 question base of this material: \n {message}" }
//                    },
//                }
//            ), Encoding.UTF8, "application/json");

//            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

//            var responseContent = await response.Content.ReadAsStringAsync();
//            var json = JsonConvert.DeserializeObject(responseContent);
//            return responseContent;
//        }
//        public async Task<string> CallChatGPTAsync2(string message, int numberOfQuestion)
//        {
//            OpenAIAPI openAIAPI = new OpenAIAPI(_apiKey);
//            var chat = openAIAPI.Chat.CreateConversation();
//            chat.Model = new Model("gpt-3.5-turbo-0125");
//            chat.AppendSystemMessage("you generate multiple answer questions base on text");
//            chat.AppendSystemMessage("Youre only response will be a JSON object with six variables:topic string, summery 1-3 sentences describing the text, a a questions object that contains: question string, options object with key value pair, answer a letter char");
            
//            chat.AppendUserInput(message+ $"\n generate {numberOfQuestion} question base on this study material");
//            //chat.AppendUserInput("Given the text Generate 5 multiple-choice questions based on the information provided, each with 4 options (A, B, C, D), where only one option is correct. text:( " + message + " )");
//            // and get the response
//            string response = await chat.GetResponseFromChatbotAsync();
//            return response;

//        }
//    }

//}
