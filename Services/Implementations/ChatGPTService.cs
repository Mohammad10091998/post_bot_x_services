using Models;
using OpenAI.Chat;
using Services.Interfaces;


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
            _chatClient = new(model: "gpt-4o", apiKey);
            _helperService = helperService;
        }

        public async Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string originalPayload, string configuredPayload, int numberOfFields)
        {
            List<(string Payload, string Description)> allPayloads = new List<(string Payload, string Description)>();
            
                string prompt = $@"
                Given the following configured payload:
 
                {configuredPayload}
 
                Each field includes:
                - Datatype: The expected type of the field (e.g., string, int).
                - Behavior: 'Fix' (constant value) or 'Random' (varies unless explicitly tested).
                - Example Value: A sample value for the field.
                - Validation Rules: Constraints on the values (e.g., cannot be null, must be greater than 0, specific format).
                **Instructions**:
                1. **For 'Fix' Behavior Fields**:
                -Use the provided 'Example Value' exactly as given for each payload. **Do not modify * *this value, no matter what.
                2. **For 'Random' Behavior Fields **:
                 - **Critical**: It is very important that values are always **distinct**, **highly random**, and **comply with the Validation Rules** for fields marked as 'Random,' unless that field is specifically being tested.
                 - Generate new and diverse values for each payload. These values must be significantly different from each other. For example, for a 'Random' string field, use different words, numbers, or patterns in each payload. For a 'Random' number field, vary the numbers widely.

                Please generate exactly 10 payloads:
                **3 Valid Payloads**: Use correct values for all fields that conform to the validation rules.
                **7 Negative Payloads**: Introduce errors by violating validation rules. Only generate payloads based on Validation Rules. Do not test the type of the field.
                
    
                **Key Rules**:
                  - **All fields must be included** in every payload.
                  - **No comments, extra information, or changes in format are allowed**.

                **Strict Output Format**:
                Adhere strictly to the following format for each payload:
                1. Payload:
                ```json
                    {{
                    ""property1"": value,
                    ""property2"": value,
                               ...
                    }}
                ```
                Description: A brief description of what this payload tests.
 
                Ensure clear separation between each payload and its description, and use appropriate values based on the configured payload's datatype and validation rules.
            ";

            var response = await _chatClient.CompleteChatAsync(prompt);

                // Validate and parse the response to extract payloads
             var payloads = _helperService.ParsePayloads(response.Value.Content[0].Text);

            return payloads;
        }


        public async Task<List<(string URL, string Description)>> GenerateURLsAsync(string baseURL, List<Params> queryParameters)
        {
            string baseUrlWithQueryParams = _helperService.GenerateFullURL(baseURL, queryParameters);

            string prompt = $@"
                Given the following URL with query parameters: {baseUrlWithQueryParams}, generate additional URLs by varying the values of the query parameters. 
                Please ensure that the new URLs cover positive, negative, and edge case scenarios for each parameter.

                **Output Requirements**:
                - Generate exactly 5 different URLs with their descriptions.
                - **No extra information, comments, or format changes are allowed**.

                **Strict Output Format**:
                - Each URL should be followed by its description in the following format:
                **Format**
                URL: generated URL
                Description: Brief description of the test scenario covered by this URL.

                Example:
                URL: https://example.com/api?param1=value1&param2=value2
                Description: Tests valid values for all parameters.

                Ensure that each URL and description is clearly separated and adhere to the format provided. Do not include any additional text, explanations, or comments.";

            var response = await _chatClient.CompleteChatAsync(prompt);

            // Validate and parse the response to extract URLs and descriptions
            var urlsWithDescription = _helperService.ParseURLs(response.Value.Content[0].Text);

            return urlsWithDescription;
        }

        public async Task<string> AnalyzeErrorAsync(string httpResponse)
        {
            string prompt = $"Analyze this HTTP error response in one line: {httpResponse}";
            var response = await _chatClient.CompleteChatAsync(prompt);
            var errorAnalysis = response.Value.Content[0].Text;
            return errorAnalysis;
            //gg
        }
    }
}
