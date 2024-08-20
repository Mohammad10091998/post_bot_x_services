using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using OpenAI.Chat;
using OpenAI;
using Services.Interfaces;
using System.ClientModel;

namespace PostBot_X_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost]
        public async Task<IActionResult> RunTest(TestModel model)
        {
            try
            {
                var response = await _testService.RunTestAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("stream-chat")]
        public async IAsyncEnumerable<string> StreamChat(TestModel model)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            ChatClient _chatClient = new(model: "gpt-3.5-turbo", apiKey); ;
            var prompt = $"Given the following configured payload:\n{model.Payload}\nPlease generate several diverse payloads, including both positive and negative scenarios. For each generated payload, provide a small description explaining what the payload is testing (e.g., edge cases, invalid data, typical use cases, etc.). Ensure that the payloads cover a wide range of scenarios to thoroughly test the API.";
            AsyncCollectionResult<StreamingChatCompletionUpdate> updates = _chatClient.CompleteChatStreamingAsync(prompt);

            Console.WriteLine($"[ASSISTANT]:");
            await foreach (StreamingChatCompletionUpdate update in updates)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    Console.WriteLine(updatePart.Text);
                    yield return updatePart.Text;
                }
            }
        }
        [HttpGet]
        [Route("GetCounter")]
        public async IAsyncEnumerable<int> GetCounter()
        {
            int i = 0;
            while (i < 100)
            {
                await Task.Delay(500);
                Console.WriteLine(i);
                i++;
                yield return i;
            }
        }
    }
}
