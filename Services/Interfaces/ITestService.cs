using Models;
using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<TestSuiteResultModel> RunAutomatedWriteTestsAsync(APITestRequestModel model);
        Task<TestSuiteResultModel> RunManualWriteTestsAsync(APITestRequestModel model);
        Task<TestSuiteResultModel> RunAutomatedReadTestsAsync(APITestRequestModel model);
        Task<TestSuiteResultModel> RunManualReadTestsAsync(APITestRequestModel model);
    }
}
