using Models;
using Services.Interfaces;
using System.Net;

namespace Services.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
        public async Task<(int StatusCode, string ResponseContent, bool IsSuccessful, string? ErrorAnalysis)> MakeApiCallAsync(
            string url,
            string? payload,
            List<Header> headers,
            string httpMethod)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(httpMethod), url);

                if (payload != null)
                {
                    request.Content = new StringContent(payload, System.Text.Encoding.UTF8, "application/json");
                }

                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.StatusCode.GetHashCode(), responseContent, response.IsSuccessStatusCode, null);
            }
            catch (HttpRequestException ex)
            {
                return ((int)HttpStatusCode.ServiceUnavailable, ex.Message, false, "Network-related error occurred during API call.");
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                return ((int)HttpStatusCode.RequestTimeout, ex.Message, false, "Request timed out.");
            }
            catch (Exception ex)
            {
                return ((int)HttpStatusCode.InternalServerError, ex.Message, false, "Unexpected error occurred.");
            }
        }
    }
}
