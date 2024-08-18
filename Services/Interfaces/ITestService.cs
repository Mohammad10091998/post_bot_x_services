using Models;
using OpenAI.Chat;

namespace Services.Interfaces
{
    public interface ITestService
    {
        Task<ChatCompletion> RunTestAsync(TestModel model);
    }
}
