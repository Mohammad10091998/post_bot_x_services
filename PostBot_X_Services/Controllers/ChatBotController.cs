using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace PostBot_X_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        public ChatBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost]
        public async Task<IActionResult> ChatBot(string query)
        {
            try
            {
                var response = await _chatBotService.UserQueryResolver(query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
