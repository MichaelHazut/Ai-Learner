
using Newtonsoft.Json;

namespace AiLearner_ClassLibrary.OpenAi_Service
{
    public static class JsonService
    {
        public static string CleanJson(string content)
        {
            //json to array of chars
            char[] charContent = content.ToCharArray();

            //get the indexes of the start and end of the json 
            int startIndex = Array.IndexOf(charContent, '{');
            int endIndex = Array.LastIndexOf(charContent, '}');

            //extract all content between the brackets
            string cleanedContent = new(charContent, startIndex, endIndex - startIndex + 1);
            return cleanedContent;

        }
        public static T? DeserializeJson<T>(string content) where T : class
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
