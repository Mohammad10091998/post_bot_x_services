using OpenAI_API.Completions;
using OpenAI_API;

namespace PostBot_X_Services
{
    public class ChatGPTService
    {
        private readonly OpenAIAPI _openAIAPI;

        public ChatGPTService(string apiKey)
        {
            _openAIAPI = new OpenAIAPI(apiKey);
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                MaxTokens = 150
            };

            var result = await _openAIAPI.Completions.CreateCompletionAsync(completionRequest);
            return result.Completions[0].Text;
        }
    }
}
