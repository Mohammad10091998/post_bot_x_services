using OpenAI;
using OpenAI.Chat;
using Services.Interfaces;

namespace Services.Implementations
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly ChatClient _chatClient;
        private readonly OpenAIClient _client;

        public ChatGPTService()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }
            _client = new(apiKey);
            _chatClient = new(model: "gpt-3.5-turbo", apiKey);
        }
        public async Task<ChatCompletion> GeneratePayloadsAsync(string configuredPayload)
        {
            var prompt = $"Given the following configured payload:\n{configuredPayload}\nPlease generate several diverse payloads, including both positive and negative scenarios. For each generated payload, provide a small description explaining what the payload is testing (e.g., edge cases, invalid data, typical use cases, etc.). Ensure that the payloads cover a wide range of scenarios to thoroughly test the API.";

            var response = await _chatClient.CompleteChatAsync(prompt);

            
            return response;
        }
        public async Task<ChatCompletion> GeneratePayloadAsync(string prompt)
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);

            return completion;
        }
        private List<(string Payload, string Description)> ParsePayloads(string chatGPTResponse)
        {
            // Logic to parse the response into a list of payloads and their descriptions
            // This will depend on how the response is formatted by ChatGPT
            // Assuming response is in a structured JSON-like format
            var result = new List<(string Payload, string Description)>();
            // Example parsing code (modify as necessary based on actual response)
            return result;
        }
    }
}
