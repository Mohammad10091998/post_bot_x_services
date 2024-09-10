
# PostBot X - Powered by Open AI

Our backend ensures lightning-fast API testing and accelerating your development cycle.

# APIs

## Request Model 
Request model is same for all API

The APITestRequestModel class is designed to facilitate the testing of APIs within our project. It accepts essential parameters such as API URL, HTTP method type, JSON schema for configured payload, and headers.

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `url` | `string` | **Required**. Your API URL |
| `apiType` | `string` | **Required**. Type of Rest API |
| `payloads` | `string` | **Required**. Stringfy Json Schema for the configured payload. Pass this as the first object in the list |
| `headers` | `key value` |  List of Key Value  |
| `queryParameters` | `key value` |  List of Key Values |

QueryParamters
| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `key` | `string` |  Query Param Key |
| `value` | `[string]` |  Array of Query Param Value |

Headers
| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `key` | `string` | Header Key  |
| `value` | `string` | Header Value  |

## Response Model : Test Results
The backend provides detailed test results in a structured JSON format. This is same for all apis Here's an explanation of the response structure:

```
{
  "testResults": [
    {
      "testData": "{\r\n  \"id\": 0,\r\n  \"name\": \"Instrument 100\",\r\n  \"description\": \"Testing is fun\",\r\n  \"internalId\": \"3254\",\r\n  \"Status\": {\r\n    \"id\": 1,\r\n    \"name\": \"InActive\"\r\n  },\r\n  \"MiddlewareRequestTestType\": {\r\n    \"id\": 2,\r\n    \"name\": \"Analyte\"\r\n  }\r\n}"
      "description": "Valid payload with all values withing constraints",
      "responseContent": "Received successful response",
      "statusCode": 200,
      "isSuccessful": true,
      "errorAnalysis": null,
      "time": "00:00:01.5000000"
    },
    {
      "testData": "{\r\n  \"id\": -1,\r\n  \"name\": \"null\",\r\n  \"description\": \"Testing\",\r\n  \"internalId\": \"1234\",\r\n  \"Status\": {\r\n    \"id\": 1,\r\n    \"name\": \"InActive\"\r\n  },\r\n  \"MiddlewareRequestTestType\": {\r\n    \"id\": 2,\r\n    \"name\": \"Analyte\"\r\n  }\r\n}",
      "description": "Testing id with negative value and name as null",
      "responseContent": "Received error response",
      "statusCode": 400,
      "isSuccessful": false,
      "errorAnalysis": "",
      "time": "00:00:00.8000000"
    }
  ]
}

```

**TestData**: Generated Payload.

**Description**: Brief description about payload 

**ResponseContent**: The response received from the testing API during the test.

**StatusCode**: The HTTP status code received in the testing API response.

**IsSuccessful**: Indicates whether the API call was successful (true) or resulted in failure (false).

**ErrorAnalysis**: If testing API throws unexpected error, analyzing it using chatgpt.

**Time**: Time taken by testing api.

This structured response helps developers quickly identify which scenarios passed and which ones failed, facilitating efficient debugging and validation of the API behavior.

## 1. Automated Write : Test Post/Put/Patch APIs

```
{
  "apiType": "POST",
  "url": "https://example.com/api/resource",
  "payload": [
    "\r\n{\r\n  \"id\": \"number, fix, 0, do not change this \",\r\n  \"name\": \"string, random, Test Post Instrument 1, not nullable\",\r\n  \"description\": \"string, fix, Testing, it's a nullable field\",\r\n  \"internalId\": \"number, fix, 1234, not null and number\",\r\n  \"Status\": {\r\n    \"id\": \"number, fix, 1, not null either 1 or 2\",\r\n    \"name\": \"string, fix, InActive, it can be nullable\"\r\n  },\r\n  \"MiddlewareRequestTestType\": {\r\n    \"id\": \"number, fix, 2, not null and either 1 or 2\",\r\n    \"name\": \"string, fix, Analyte, it is nullable\"\r\n  }\r\n}"
  ],
  "headers": [
    {
      "key": "Authorization",
      "value": "Bearer your_access_token"
    },
    {
      "key": "Content-Type",
      "value": "application/json"
    }
  ],
}

```

## How to Configure a Payload for API Testing
To effectively test your APIs, it's crucial to configure the payloads correctly. The payload configuration format allows for specifying data types, behavior, sample values, and validation rules for each field in your API request. This configuration is designed to help generate comprehensive and robust payloads that cover all test scenarios.

### Understanding the Payload Configuration Format
Each field in the payload configuration follows a structured format with four components:

**Data Type:** Specifies the type of data for the field (e.g., number, string).

**Behavior:** Determines how the field's value will behave across generated payloads. It can be:

**fix:** The value for the field remains constant across all generated payloads, except when the field itself is being tested.

**random:** The value for the field will be distinct across all generated payloads, ensuring varied testing scenarios.
Sample Value: Provides a sample value to help generate realistic payloads. This value is used when the behavior is set to fix.

**Validation Rules (Optional):** A plain English description of the validation rules or constraints for the field. This helps in generating payloads that adhere to the field's constraints. The validation rules can include conditions like "not nullable," "either 1 or 2," or "should be a positive number."

#### Configure Payload
```
{
  "id": "number, fix, 0, do not change this ",
  "name": "string, random, Test Instrument 1, not nullable",
  "description": "string, fix, Testing, it's a nullable field",
  "internalId": "number, fix, 1234, not null and number",
  "Status": {
    "id": "number, fix, 1, not null either 1 or 2",
    "name": "string, fix, InActive, it can be nullable"
  },
  "MiddlewareRequestTestType": {
    "id": "number, fix, 2, not null and either 1 or 2",
    "name": "string, fix, Analyte, it is nullable"
  }
}
```

### Payload Generation using chatgpt model:
The backend system leverages the capabilities of the ChatGPT model for dynamic payload generation.


## 2. Manual Write : Test Post/Put/Patch APIs
This API provides users with the capability to manually test Post/Put/Patch APIs by allowing them to supply multiple payloads in a single request. This enables comprehensive testing of the API with various data sets, ensuring that different scenarios are adequately covered. The API will be tested against each provided payload, allowing users to validate their API's behavior under different conditions.

Example
```
{
  "url": "https://example.com/api/resource",
  "apiType": "POST",
  "payloads": [
    {
      "value": "{\"name\":\"John Doe\",\"age\":25,\"email\":\"john.doe@example.com\",\"address\":{\"street\":\"123 Main St\",\"city\":\"Cityville\",\"zip\":\"12345\"}}"
    },
    {
      "value": "{\"name\":\"Jane Smith\",\"age\":30,\"email\":\"jane.smith@example.com\",\"address\":{\"street\":\"456 Oak St\",\"city\":\"Townsville\",\"zip\":\"54321\"}}"
    },
    // Add more JSON schemas as needed
  ],
  "headers": [
    {
      "key": "Authorization",
      "value": "Bearer your_access_token"
    },
    {
      "key": "Content-Type",
      "value": "application/json"
    }
  ]
}

```

## 3. Automated Read : Test Get/Delete APIs
For testing Get/Del APIs, you can provide query parameters. Will harness chatgpt for testing the query parameters for different scenario. Here's how you can structure your request:

```
{
  "url": "https://example.com/api/resource",
  "apiType": "GET",
  "headers": [
    {
      "key": "Authorization",
      "value": "Bearer your_access_token"
    },
    {
      "key": "Content-Type",
      "value": "application/json"
    }
  ],
  "queryParameters": [
    {
      "key": "name",
      "value": ["John"]
    },
    {
      "key": "age",
      "value": ["30"]
    },
    // Add more query parameters as needed
  ]
}
```
**Example Usage:**
```
{
  "key": "name",
  "value": "John"
}
```
In this example, the user defines a query parameter named "name" with the value "John." 

## 4. Manual Read : Test Get/Delete APIs
This API provides users with the capability to manually test Get/Delete APIs by allowing them to supply multiple values for each query parameter in a single request. This enables comprehensive testing of the API with various data sets, ensuring that different scenarios are adequately covered. 

```
{
  "url": "https://example.com/api/resource",
  "apiType": "GET",
  "headers": [
    {
      "key": "Authorization",
      "value": "Bearer your_access_token"
    },
    {
      "key": "Content-Type",
      "value": "application/json"
    }
  ],
  "paramPairs": [
    {
      "key": "name",
      "value": ["John", "Jason", ""]
    },
    {
      "key": "age",
      "value": ["30", "25", ""]
    },
    // Add more query parameters with multiple values as needed
  ]
}
```
**Example Usage:**
Users can specify query parameters with different values, and our backend will test the API using each provided value. For example:
```
{
  "key": "name",
  "values": ["John", "Jason", ""]
}
```
In this example, the user defines a query parameter named "name" with multiple values. Our backend will then test the Get/Delete API using each value provided for the "name" parameter.

## Future Release 
### 1. Chatbot
In our upcoming release, we are excited to integrate the ChatGPT model to enhance user interactions with our application. This integration will allow the model to Answer User Queries. Provide instant, accurate responses to user questions regarding the application, its features, and usage. Our team is actively working on fine-tuning the ChatGPT model to ensure it meets our specific needs. This fine-tuning process involves

### 2. Load Testing
We are also preparing to add feature of comprehensive load testing of the APIs.



