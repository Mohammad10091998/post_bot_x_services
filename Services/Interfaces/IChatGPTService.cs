using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string configuredPayload);
        Task<List<(string URL, string Description)>> GenerateURLsAsync(string baseURL, List<Models.Params> queryParameters);
        Task<string> AnalyzeErrorAsync(string httpResponse);
    }
}
