
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
            string cleanedContent = new string(charContent, startIndex, endIndex - startIndex + 1);
            return cleanedContent;
        }

        public static string FixMissingBracket(string json)
        {
            // Find the last occurrence of "answer"
            int lastAnswerIndex = json.LastIndexOf("\"answer\"");
            if (lastAnswerIndex == -1)
            {
                // "answer" not found, return the original json
                return json;
            }

            // Find the index of the colon after the last occurrence of "answer"
            int lastColonIndex = json.IndexOf(":", lastAnswerIndex);
            if (lastColonIndex == -1)
            {
                // Colon not found, return the original json
                return json;
            }

            // Find the index of the last comma or closing bracket after the last colon, which marks the end of the answer value
            int nextCommaIndex = json.IndexOf(",", lastColonIndex);
            int nextBracketIndex = json.IndexOf("]", lastColonIndex);
            int insertIndex = (nextCommaIndex == -1) ? nextBracketIndex : Math.Min(nextCommaIndex, nextBracketIndex);

            if (insertIndex == -1)
            {
                // Neither a comma nor a closing bracket was found, return the original json
                return json;
            }

            // Insert the closing brace before the comma or the closing bracket
            return json.Substring(0, insertIndex) + "}" + json.Substring(insertIndex);
        }
    }
}
