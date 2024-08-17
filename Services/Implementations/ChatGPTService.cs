using OpenAI;
using OpenAI.Chat;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly ChatClient _chatClient;

        public ChatGPTService()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }

            _chatClient = new(model: "gpt-3.5-turbo", apiKey);
        }

        public async Task<ChatCompletion> GeneratePayloadAsync(string prompt)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);

            return completion;
        }
    }
}
