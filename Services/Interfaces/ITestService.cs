using Models;
using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<TestSuiteResultModel> RunAutomatedWriteTestsAsync(TestModel model);
        Task<TestSuiteResultModel> RunManualWriteTestsAsync(TestModel model);
        Task<TestSuiteResultModel> RunAutomatedReadTestsAsync(TestModel model);
        Task<TestSuiteResultModel> RunManualReadTestsAsync(TestModel model);
    }
}
