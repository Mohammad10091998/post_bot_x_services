using Models;

namespace Services.Interfaces
{
    public interface IHelperService
    {
        List<(string Payload, string Description)> ParsePayloads(string input);
        List<(string URL, string Description)> GenerateTestUrls(string baseUrl, List<KeyValue> queryParameters);
        public string GenerateFullURL(string baseUrl, List<KeyValue> queryParameters);
        List<(string URL, string Description)> ParseURLs(string input);
    }
}
