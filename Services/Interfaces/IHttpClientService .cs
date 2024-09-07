using Models;

namespace Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<(int StatusCode, string ResponseContent, bool IsSuccessful, string? ErrorAnalysis, TimeSpan TimeTaken)> MakeApiCallAsync(string url,string? payload,List<Header> headers,string httpMethod);
    }
}
