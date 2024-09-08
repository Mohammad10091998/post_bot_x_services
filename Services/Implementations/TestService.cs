using Models;
using Services.Interfaces;
using System.Net;

namespace Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly IChatGPTService _chatGPTService;
        private readonly IHttpClientService _httpClientService;
        private readonly IHelperService _helperService;
        public TestService(IChatGPTService chatGPTService, IHttpClientService httpClientService, IHelperService helperService)
        {
            _chatGPTService = chatGPTService;
            _httpClientService = httpClientService;
            _helperService = helperService;
        }
        public async Task<TestSuiteResultModel> RunAutomatedWriteTestsAsync(APITestRequestModel model, CancellationToken cancellationToken)
        {
            int numOfFields = model.NumberOfFields != null ? model.NumberOfFields.Value : 10;
            if(model.Payload == null)
            {
                throw new InvalidDataException("Configure Payload cannot be Null");
            }
            var payloadsWithDescriptions = await _chatGPTService.GeneratePayloadsAsync(model.Payload.First(), cancellationToken);

            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            
            foreach (var (payload, description) in payloadsWithDescriptions)
            {
                var result = await _httpClientService.MakeApiCallAsync(model.Url, payload, model.Headers, model.ApiType, cancellationToken);
                string errorAnalysis = string.Empty;
                if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    errorAnalysis = await _chatGPTService.AnalyzeErrorAsync(result.ResponseContent);
                }
                testResults.Add(new TestResultResponseModel
                {
                    TestData = payload,
                    Description = description,
                    StatusCode = result.StatusCode,
                    ResponseContent = result.ResponseContent,
                    IsSuccessful = result.IsSuccessful,
                    ErrorAnalysis = errorAnalysis,
                    Time = result.TimeTaken,
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunManualWriteTestsAsync(APITestRequestModel model, CancellationToken cancellationToken)
        {  
            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            if (model.Payload != null && model.Payload.Any())
            {
                foreach (var payload in model.Payload)
                {
                    var result = await _httpClientService.MakeApiCallAsync(model.Url, payload, model.Headers, model.ApiType, cancellationToken);
                    string errorAnalysis = string.Empty;
                    if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
                    {
                        errorAnalysis = await _chatGPTService.AnalyzeErrorAsync(result.ResponseContent);
                    }
                    testResults.Add(new TestResultResponseModel
                    {
                        TestData = payload,
                        Description = "",
                        StatusCode = result.StatusCode,
                        ResponseContent = result.ResponseContent,
                        IsSuccessful = result.IsSuccessful,
                        ErrorAnalysis = errorAnalysis,
                        Time = result.TimeTaken,
                    });
                } 
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunAutomatedReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken)
        {
            var urlsWithDescriptions = await _chatGPTService.GenerateURLsAsync(model.Url, model.QueryParameters, cancellationToken);

            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            foreach (var (url, description) in urlsWithDescriptions)
            {
                var result = await _httpClientService.MakeApiCallAsync(url, model.Payload?.FirstOrDefault(), model.Headers, model.ApiType, cancellationToken);
                string errorAnalysis = string.Empty;
                if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    errorAnalysis = await _chatGPTService.AnalyzeErrorAsync(result.ResponseContent);
                }
                testResults.Add(new TestResultResponseModel
                {
                    TestData = url,
                    Description = description,
                    StatusCode = result.StatusCode,
                    ResponseContent = result.ResponseContent,
                    IsSuccessful = result.IsSuccessful,
                    ErrorAnalysis = errorAnalysis,
                    Time = result.TimeTaken,
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunManualReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken)
        {
            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            var urlsWithDescription = _helperService.GenerateTestUrls(model.Url, model.QueryParameters);
            foreach (var (url, description) in urlsWithDescription)
            {
                var result = await _httpClientService.MakeApiCallAsync(url, model.Payload?.FirstOrDefault(), model.Headers, model.ApiType, cancellationToken);
                string errorAnalysis = string.Empty;
                if (result.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    errorAnalysis = await _chatGPTService.AnalyzeErrorAsync(result.ResponseContent);
                }
                testResults.Add(new TestResultResponseModel
                {
                    TestData = url,
                    Description = description,
                    StatusCode = result.StatusCode,
                    ResponseContent = result.ResponseContent,
                    IsSuccessful = result.IsSuccessful,
                    ErrorAnalysis = errorAnalysis,
                    Time = result.TimeTaken,
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
    }
}
