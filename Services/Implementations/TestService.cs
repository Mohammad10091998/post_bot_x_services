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
        public async Task<TestSuiteResultModel> RunAutomatedWriteTestsAsync(TestModel model)
        {
            var payloadsWithDescriptions = await _chatGPTService.GeneratePayloadsAsync(model.Payload.First());

            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            
            foreach (var (payload, description) in payloadsWithDescriptions)
            {
                var result = await _httpClientService.MakeApiCallAsync(model.Url, payload, model.Headers, model.ApiType);
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
                    ErrorAnalysis = errorAnalysis
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunManualWriteTestsAsync(TestModel model)
        {  
            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            foreach (var payload in model.Payload)
            {
                var result = await _httpClientService.MakeApiCallAsync(model.Url, payload, model.Headers, model.ApiType);
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
                    ErrorAnalysis = errorAnalysis
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunAutomatedReadTestsAsync(TestModel model)
        {
            var urlsWithDescriptions = await _chatGPTService.GenerateURLsAsync(model.Url, model.QueryParameters);

            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            foreach (var (url, description) in urlsWithDescriptions)
            {
                var result = await _httpClientService.MakeApiCallAsync(url, model.Payload.FirstOrDefault(), model.Headers, model.ApiType);
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
                    ErrorAnalysis = errorAnalysis
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
        public async Task<TestSuiteResultModel> RunManualReadTestsAsync(TestModel model)
        {
            var testResults = new List<TestResultResponseModel>();
            var testSuiteResult = new TestSuiteResultModel();
            var urlsWithDescription = _helperService.GenerateTestUrls(model.Url, model.QueryParameters);
            foreach (var (url, description) in urlsWithDescription)
            {
                var result = await _httpClientService.MakeApiCallAsync(url, model.Payload.FirstOrDefault(), model.Headers, model.ApiType);
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
                    ErrorAnalysis = errorAnalysis
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
    }
}
