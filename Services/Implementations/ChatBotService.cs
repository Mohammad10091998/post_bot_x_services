using OpenAI.Chat;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ChatBotService : IChatBotService
    {
        private readonly ChatClient _chatClient;
        public ChatBotService()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }
            _chatClient = new(model: "ft:gpt-3.5-turbo-0125:personal:postbot-x-chat:A335SCFJ", apiKey);
        }
        public async Task<string> UserQueryResolver(string query)
        {
            var response = await _chatClient.CompleteChatAsync(query);
            var answer = response.Value.Content[0].Text;
            return answer;
        }
    }
}
