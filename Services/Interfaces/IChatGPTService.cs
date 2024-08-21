using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string configuredPayload);
        Task<string> AnalyzeErrorAsync(string httpResponse);
    }
}
