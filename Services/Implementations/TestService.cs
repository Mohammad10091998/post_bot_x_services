using Models;
using Services.Interfaces;

namespace Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly IChatGPTService _chatGPTService;
        private readonly IHttpClientService _httpClientService;
        public TestService(IChatGPTService chatGPTService, IHttpClientService httpClientService)
        {
            _chatGPTService = chatGPTService;
            _httpClientService = httpClientService;
        }
        public async Task<TestSuiteResultModel> RunTestAsync(TestModel model)
        {

            var payloadsWithDescriptions = await _chatGPTService.GeneratePayloadsAsync(model.Payload);
            var testResults = new List<TestResultModel>();
            var testSuiteResult = new TestSuiteResultModel
            {
                ApiUrl = model.Url,
                ApiType = model.ApiType
            };
            foreach (var (payload, description) in payloadsWithDescriptions)
            {
                var result = await _httpClientService.MakeApiCallAsync(model.Url, payload, model.HeaderPairs, model.ApiType);
                testResults.Add(new TestResultModel
                {
                    Payload = payload,
                    PayloadDescription = description,
                    StatusCode = result.StatusCode,
                    ResponseContent = result.ResponseContent,
                    IsSuccessful = result.IsSuccessful
                });
            }
            testSuiteResult.TestResults = testResults;
            return testSuiteResult;
        }
    }
}
