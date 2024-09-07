using Models;
using Services.Interfaces;
using System.Net;
using System.Diagnostics; 

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

        public async Task<(int StatusCode, string ResponseContent, bool IsSuccessful, string? ErrorAnalysis, TimeSpan TimeTaken)> MakeApiCallAsync(
            string url,
            string? payload,
            List<Header> headers,
            string httpMethod)
        {
            Stopwatch stopwatch = new Stopwatch(); // Initialize Stopwatch

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

                stopwatch.Start(); // Start timing
                var response = await _httpClient.SendAsync(request);
                stopwatch.Stop(); // Stop timing after response is received

                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.StatusCode.GetHashCode(), responseContent, response.IsSuccessStatusCode, null, stopwatch.Elapsed);
            }
            catch (HttpRequestException ex)
            {
                stopwatch.Stop(); // Ensure stopwatch is stopped on error
                return ((int)HttpStatusCode.ServiceUnavailable, ex.Message, false, "Network-related error occurred during API call.", stopwatch.Elapsed);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                stopwatch.Stop(); // Ensure stopwatch is stopped on timeout
                return ((int)HttpStatusCode.RequestTimeout, ex.Message, false, "Request timed out.", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                stopwatch.Stop(); // Ensure stopwatch is stopped on unexpected error
                return ((int)HttpStatusCode.InternalServerError, ex.Message, false, "Unexpected error occurred.", stopwatch.Elapsed);
            }
        }
    }
}
