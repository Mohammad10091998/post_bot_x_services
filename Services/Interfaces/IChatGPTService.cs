using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<ChatCompletion> GeneratePayloadAsync(string prompt);
    }
}
