using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace PostBot_X_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatGPTService _chatGPTService;

        public ChatController(IChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string prompt)
        {
            try
            {
                var response = await _chatGPTService.GeneratePayloadAsync(prompt);
                return Ok(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
