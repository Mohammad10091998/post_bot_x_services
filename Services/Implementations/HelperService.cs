using Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Services.Interfaces;
using System.Text.RegularExpressions;

namespace Services.Implementations
{
    public class HelperService : IHelperService
    {
        public List<(string Payload, string Description)> ParsePayloads(string responseText)
        {
            // Improved regex pattern to match JSON blocks and their descriptions
            var regex = new Regex(
                @"(?<=\d+\.\s*Payload:\s*)```json\s*(\{.+?\})\s*```\s*Description:\s*(.+?)(?=\d+\.\s*Payload:|\Z)",
                RegexOptions.Singleline | RegexOptions.IgnoreCase
            );

            var matches = regex.Matches(responseText);
            var payloads = new List<(string Payload, string Description)>();

            foreach (Match match in matches)
            {
                var payload = match.Groups[1].Value.Trim();
                var description = match.Groups[2].Value.Trim();

                // Additional validation to ensure valid JSON
                if (IsValidJson(payload))
                {
                    payloads.Add((payload, description));
                }
            }

            return payloads;
        }

        // Helper function to validate JSON string format
        private bool IsValidJson(string payload)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<JObject>(payload);
                return obj != null;
            }
            catch
            {
                return false;
            }
        }

        public List<(string URL, string Description)> GenerateTestUrls(string baseUrl, List<Params> queryParameters)
        {
            var generatedUrlsWithDescription = new List<(string Payload, string Description)>();

            if (queryParameters == null || !queryParameters.Any())
            {
                generatedUrlsWithDescription.Add((baseUrl, ""));
                return generatedUrlsWithDescription;
            }

            // Take the first value from each query parameter and build the base URL with them
            string baseUrlWithQueryParams = GenerateFullURL(baseUrl, queryParameters);
            

            // Generate URLs by replacing one query parameter's value at a time
            foreach (var param in queryParameters)
            {
                foreach (var value in param.Value)
                {
                    var url = ReplaceQueryParam(baseUrlWithQueryParams, param.Key, value);
                    generatedUrlsWithDescription.Add((url, $"Tested Parameter: {param.Key}, Value: {value}"));
                }
            }

            return generatedUrlsWithDescription;
        }
        public string GenerateFullURL(string baseUrl, List<Params> queryParameters)
        {
            string baseUrlWithQueryParams = baseUrl;
            foreach (var param in queryParameters)
            {
                if (param.Value.Any())
                {
                    baseUrlWithQueryParams = AppendQueryParam(baseUrlWithQueryParams, param.Key, param.Value[0]);
                }
            }
            return baseUrlWithQueryParams;
        }
        private string AppendQueryParam(string url, string key, string value)
        {
            return url.Contains("?") ? $"{url}&{key}={value}" : $"{url}?{key}={value}";
        }

        private string ReplaceQueryParam(string url, string key, string value)
        {
            var regex = new System.Text.RegularExpressions.Regex($"{key}=[^&]*");
            return regex.Replace(url, $"{key}={value}");
        }
        public List<(string URL, string Description)> ParseURLs(string input)
        {
            var urlsWithDescription = new List<(string URL, string Description)>();

            var regex = new Regex(@"URL:\s*(\S+)\s*Description:\s*(.+?)(?=(URL:|\Z))", RegexOptions.Singleline);
            var matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                var url = match.Groups[1].Value.Trim();
                var description = match.Groups[2].Value.Trim();
                urlsWithDescription.Add((url, description));
            }

            return urlsWithDescription;
        }
    }
}
