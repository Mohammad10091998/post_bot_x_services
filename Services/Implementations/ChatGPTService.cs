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
            _chatClient = new(model: "gpt-4", apiKey);
            _helperService = helperService;
        }

        public async Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string originalPayload, string configuredPayload, int numberOfFields)
        {
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
                Given the configured payload:

                ```json
                {configuredPayload}
                ```

                Each field in the payload has the following properties:
                - **Datatype**: Type of the field (e.g., string, int).
                - **Behavior**: 'Fix' (value remains constant) or 'Unique' (value must be unique across different payloads).
                - **Example Value**: Sample value for the field that should be used as a reference for uniqueness.
                - **Validation Rules**: Constraints that define valid values for the field.

                **Objective**:
                Generate up to 3 payloads to test the `{key}` field only. Generate values for this field that violate its Validation Rules. Do **not** test any other field. **Ensure all 'Unique' fields have distinct random values for each payload.**

                **Instructions**:

                1. **For 'Fix' Behavior Fields**:
                   - Use the provided Example Value exactly as given for each payload. **Do not modify** this value, no matter what.

                2. **For 'Unique' Behavior Fields**:
                   - **Critical**: It is very important that values are always distinct, very random, and comply with the Validation Rules for fields marked as 'Unique,' unless that field is specifically being tested. If there are no rules, use appropriate random values.
    
                3. **Validation Rules**:
                   - Focus only on testing the `{key}` field according to its validation rules (if provided). Create payloads that cover positive, negative, and edge cases for the `{key}` field. If this field does not have any validation rules, then generate values for it according to your discretion. **Do not modify any other field** unless necessary.

               **Key Rules**:
                    - **All fields must be included** in every payload.
                    - **No comments, extra information, or changes in format are allowed**.

                **Strict Output Format**:
                Generate payloads only in plain text format. Adhere strictly to the following format for each payload:

                ```json
                {{
                    ""property1"": value,
                    ""property2"": value,
                    ...
                }}
                ```
                ";

                // Call ChatGPT to generate payloads for the current key
                var response = await _chatClient.CompleteChatAsync(prompt);

                // Validate and parse the response to extract payloads
                var partialPayloads = _helperService.ParsePayloads(response.Value.Content[0].Text);
                allPayloads.AddRange(partialPayloads);
                
            }

            return allPayloads;
        }

        static void TraverseExpandoObject(IDictionary<string, object> dictionary, List<string> keysList, string parentKey = "parentobject")
        {
            foreach (var kvp in dictionary)
            {
                string currentKey = $"{parentKey}.{kvp.Key}";

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
                    keysList.Add(currentKey);
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
