using Models;

namespace Services.Interfaces
{
    public interface IHelperService
    {
        List<(string Payload, string Description)> ParsePayloads(string input);
        List<(string URL, string Description)> GenerateTestUrls(string baseUrl, List<Params>? queryParameters);
        public string GenerateFullURL(string baseUrl, List<Params>? queryParameters);
        List<(string URL, string Description)> ParseURLs(string input);
    }
}
