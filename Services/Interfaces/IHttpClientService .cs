using Models;

namespace Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<(int StatusCode, string ResponseContent, bool IsSuccessful, string? ErrorAnalysis)> MakeApiCallAsync(string url,string? payload,List<KeyValue> headerPairs,string httpMethod);
    }
}
