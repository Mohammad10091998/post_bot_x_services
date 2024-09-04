using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string orinalPayload, string configuredPayload, int numberOfFields);
        //Task<List<(string Payload, string Description)>> ChunkPayloadGeneration(string configuredPayload, int numberOfFields);
        Task<List<(string URL, string Description)>> GenerateURLsAsync(string baseURL, List<Models.Params> queryParameters);
        Task<string> AnalyzeErrorAsync(string httpResponse);
    }
}
