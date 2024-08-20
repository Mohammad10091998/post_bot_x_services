using Services.Interfaces;
using System.Text.RegularExpressions;

namespace Services.Implementations
{
    public class HelperService : IHelperService
    {
        public List<(string Payload, string Description)> ParsePayloads(string responseText)
        {
            // Define regex pattern to match each payload and its description
            var regex = new Regex(@"\d+\.\s*Payload:\s*(\{.+?\})\s*Description:\s*(.+?)(?=\d+\.\s*Payload:|\Z)", RegexOptions.Singleline);
            var matches = regex.Matches(responseText);

            var payloads = new List<(string Payload, string Description)>();

            foreach (Match match in matches)
            {
                // Extract payload and description from each match
                var payload = match.Groups[1].Value.Trim();
                var description = match.Groups[2].Value.Trim();
                payloads.Add((payload, description));
            }

            return payloads;
        }
    }
}
