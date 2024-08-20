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
            _chatClient = new(model: "gpt-3.5-turbo", apiKey);
            _helperService = helperService;
        }
        public async Task<List<(string Payload, string Description)>> GeneratePayloadsAsync(string configuredPayload)
        {

            string prompt = $@"
                Given this configured payload:

                {configuredPayload}

                Each field includes:
                - Datatype: Field type (e.g., string, int).
                - Behavior: 'Fix' (value stays constant) or 'Random' (value changes similarly unless being tested).
                - Example Value: A sample value for the field.
                - Validation Rules: Optional constraints on field values.

                Generate diverse payloads covering positive, negative, and edge cases. Use this format:

                1. Payload:
                {{
                    ""property1"": value,
                    ""property2"": value,
                    ...
                }}
                Description: Very small description what this payload tests.

                Ensure clear separation between each payload and description.
            ";

            var response = await _chatClient.CompleteChatAsync(prompt);
            //var Text = "1. Payload:\r\n{\r\n    \"orderId\": 12345,\r\n    \"customer\": {\r\n        \"customerId\": \"C001\",\r\n        \"name\": \"John Doe\",\r\n        \"email\": \"john.doe@example.com\",\r\n        \"address\": {\r\n            \"street\": \"123 Elm Street\",\r\n            \"city\": \"Springfield\",\r\n            \"state\": \"IL\",\r\n            \"zipCode\": 62704\r\n        }\r\n    },\r\n    \"items\": [\r\n        {\r\n            \"itemId\": \"I001\",\r\n            \"productName\": \"Widget A\",\r\n            \"quantity\": 2,\r\n            \"price\": 19.99\r\n        }\r\n    ]\r\n}\r\nDescription: Basic payload with all fields filled with valid values.\r\n\r\n2. Payload:\r\n{\r\n    \"orderId\": -1,\r\n    \"customer\": {\r\n        \"customerId\": \"Invalid\",\r\n        \"name\": \"John Doe\",\r\n        \"email\": \"john.doe@example.com\",\r\n        \"address\": {\r\n            \"street\": \"123 Elm Street\",\r\n            \"city\": \"Springfield\",\r\n            \"state\": \"IL\",\r\n            \"zipCode\": 62704\r\n        }\r\n    },\r\n    \"items\": [\r\n        {\r\n            \"itemId\": \"Invalid\",\r\n            \"productName\": \"Widget B\",\r\n            \"quantity\": 0,\r\n            \"price\": -1.99\r\n        }\r\n    ]\r\n}\r\nDescription: Payload with negative values for orderId, quantity, and price, and invalid values for customerId and itemId.\r\n\r\n3. Payload:\r\n{\r\n    \"orderId\": 0,\r\n    \"customer\": {\r\n        \"customerId\": \"C0\",\r\n        \"name\": \"John Doe\",\r\n        \"email\": \"john.doe@example.com\",\r\n        \"address\": {\r\n            \"street\": \"12\",\r\n            \"city\": \"Springfield\",\r\n            \"state\": \"IL\",\r\n            \"zipCode\": 123456\r\n        }\r\n    },\r\n    \"items\": []\r\n}\r\nDescription: Payload testing edge cases for orderId, customerId, address street length, and zipCode.\r\n\r\n4. Payload:\r\n{\r\n    \"orderId\": 12345,\r\n    \"customer\": {\r\n        \"customerId\": \"C001\",\r\n        \"name\": \"\",\r\n        \"email\": \"invalid_email\",\r\n        \"address\": {\r\n            \"street\": \"123 Elm Street\",\r\n            \"city\": \"Springfield\",\r\n            \"state\": \"IL\",\r\n            \"zipCode\": 62704\r\n        }\r\n    },\r\n    \"items\": [\r\n        {\r\n            \"itemId\": \"I001\",\r\n            \"productName\": null,\r\n            \"quantity\": 1000,\r\n            \"price\": 0\r\n        }\r\n    ]\r\n}\r\nDescription: Payload with empty name, invalid email address, null productName, maximum quantity, and price equal to 0.";
            var payloads = _helperService.ParsePayloads(response.Value.Content[0].Text);
            //var payloads = _helperService.ParsePayloads(Text);
            return payloads;
        }
       
    }
}
