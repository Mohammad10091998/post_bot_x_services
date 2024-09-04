using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAI.Chat;
using Services.Interfaces;
using System;
using System.ClientModel;
using System.Collections;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Xml.Linq;

namespace Services.Implementations
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly ChatClient _chatClient;
        private readonly IHelperService _helperService;

        public ChatGPTService(IHelperService helperService)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key is not configured.");
            }
            _chatClient = new(model: "gpt-3.5-turbo", apiKey);
            _helperService = helperService;
        }

        public async Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string originalPayload, string configuredPayload, int numberOfFields)
        {
            int numberOfPayloads = 2;  // Number of payloads to generate for each key; adjust as needed.
            List<(string Payload, string Description)> allPayloads = new List<(string Payload, string Description)>();
            var converter = new ExpandoObjectConverter();
            dynamic configureJson = JsonConvert.DeserializeObject<ExpandoObject>(configuredPayload, converter);
            dynamic originalJson = JsonConvert.DeserializeObject<ExpandoObject>(originalPayload, converter);
            var dictionary = (IDictionary<string, object>)configureJson;
            List<string> keysList = new();
            TraverseExpandoObject(dictionary, keysList);

            foreach (var key in keysList)
            {
                string prompt = $@"
                Given this configured payload:

                {configuredPayload}

                Each field includes:
                - Datatype: Field type (e.g., string, int).
                - Behavior: 'Fix' (value stays constant as) or 'Random' (value changes similarly unless being tested).
                - Example Value: A sample value for the field.
                - Validation Rules: It is constraint on the fields and against which we need to test these field.
                 
               

                **Instructions**
                -Generate payloads to test the {key} field only, covering all negative scenarios. Generate payloads as minimum as possible to cover all edge cases.
                -Include all the fields and set their value according to their behaviour.

                **Strictly follow this format**
                1. Payload:
                {{
                    ""property1"": value,
                    ""property2"": value,
                    ...
                }}
            ";

                // Call ChatGPT to generate payloads for the current key
                var response = await _chatClient.CompleteChatAsync(prompt);

                // Parse the response to extract payloads
                var partialPayloads = _helperService.ParsePayloads(response.Value.Content[0].Text);

                allPayloads.AddRange(partialPayloads);
            }

            return allPayloads;
        }

        static void TraverseExpandoObject(IDictionary<string, object> dictionary, List<string> keysList, string parentKey = "")
        {
            foreach (var kvp in dictionary)
            {
                string currentKey = string.IsNullOrEmpty(parentKey) ? kvp.Key : $"{parentKey}.{kvp.Key}";

                if (kvp.Value is IDictionary<string, object> nestedDict)
                {
                    // If the value is a nested ExpandoObject, recurse
                    TraverseExpandoObject(nestedDict, keysList, currentKey);
                }
                else if (kvp.Value is IList nestedList)
                {
                    // If the value is a list or array, iterate through its items
                    for (int i = 0; i < nestedList.Count; i++)
                    {
                        if (nestedList[i] is IDictionary<string, object> listDict)
                        {
                            TraverseExpandoObject(listDict, keysList , $"{currentKey}[{i}]");
                        }
                        else
                        {
                            Console.WriteLine($"Key: {currentKey}[{i}], Value: {nestedList[i]}");
                        }
                    }
                }
                else
                {
                    // If the value is a primitive type, just print it
                    keysList.Add(kvp.Key);
                    Console.WriteLine($"Key: {currentKey}, Value: {kvp.Value}");
                }
            }
        }
        public async Task<List<(string URL, string Description)>> GenerateURLsAsync(string baseURL, List<Params> queryParameters)
        {
            string baseUrlWithQueryParams = _helperService.GenerateFullURL(baseURL, queryParameters);
            string prompt = $"Given the following URL with query parameters: {baseUrlWithQueryParams}, " +
                $"generate additional URLs by varying the values of the query parameters. " +
                $"Please ensure that the new URLs cover positive, negative, and edge case scenarios for each parameter." +
                $"\r\n\r\nFormat the response as follows:\r\n\r\nURL: generated URL\r\nDescription: Brief description of the test scenario covered by this URL." +
                $"\r\nGenerate at least 5 different URLs with their descriptions.";
            var response = await _chatClient.CompleteChatAsync(prompt);
            var urlsWithDescription = _helperService.ParseURLs(response.Value.Content[0].Text);
            return urlsWithDescription;
        }
        public async Task<string> AnalyzeErrorAsync(string httpResponse)
        {
            string prompt = $"Analyze this HTTP error response in one line: {httpResponse}";
            var response = await _chatClient.CompleteChatAsync(prompt);
            var errorAnalysis = response.Value.Content[0].Text;
            return errorAnalysis;
        }
    }
}
