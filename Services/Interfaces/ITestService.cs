using Models;
using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<TestSuiteResultModel> RunTestAsync(TestModel model);
    }
}
