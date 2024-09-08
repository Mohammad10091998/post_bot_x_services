using Models;
using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<TestSuiteResultModel> RunAutomatedWriteTestsAsync(APITestRequestModel model, CancellationToken cancellationToken);
        Task<TestSuiteResultModel> RunManualWriteTestsAsync(APITestRequestModel model, CancellationToken cancellationToken);
        Task<TestSuiteResultModel> RunAutomatedReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken);
        Task<TestSuiteResultModel> RunManualReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken);
    }
}
