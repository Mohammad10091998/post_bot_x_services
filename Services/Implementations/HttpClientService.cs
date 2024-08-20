using Models;
using Services.Interfaces;

namespace Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<(int StatusCode, string ResponseContent, bool IsSuccessful, string? ErrorAnalysis)> MakeApiCallAsync(
            string url,
            string? payload,
            List<KeyValue> headerPairs,
            string httpMethod)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(httpMethod), url);

                if (payload != null)
                {
                    request.Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
                }

                foreach (var header in headerPairs)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.StatusCode.GetHashCode(), responseContent, response.IsSuccessStatusCode, null);
            }
            catch (HttpRequestException ex)
            {
                return (0, ex.Message, false, "Error occurred during API call.");
            }
            catch (Exception ex)
            {
                return (0, ex.Message, false, "Unexpected error occurred.");
            }
        }
    }
}
