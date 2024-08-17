using OpenAI.Chat;

namespace PostBot_X_Services
{
    public class ChatGPTService
    {
        private readonly ChatClient _chatClient;
        public ChatGPTService()
        {
            _chatClient = new(model: "gpt-3.5-turbo", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        }

        public async Task<ChatCompletion> GetResponseAsync(string prompt)
        {
            ChatCompletion completion = _chatClient.CompleteChat(prompt);

            return completion;
        }
    }
}
