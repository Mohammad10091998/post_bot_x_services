using Models;
using OpenAI.Chat;
using Services.Interfaces;
using System.Security.AccessControl;

namespace Services.Implementations
{
    public class TestService : ITestService
    {
        private readonly IChatGPTService _chatGPTService;
        public TestService(IChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
        }
        public async Task<ChatCompletion> RunTestAsync(TestModel model)
        {

            ChatCompletion payloadsWithDescriptions = await _chatGPTService.GeneratePayloadsAsync(model.Payload);

            return payloadsWithDescriptions;
        }
    }
}
