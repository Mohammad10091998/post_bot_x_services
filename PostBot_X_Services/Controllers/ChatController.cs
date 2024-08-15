﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PostBot_X_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatGPTService _chatGPTService;

        public ChatController(ChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string prompt)
        {
            var response = await _chatGPTService.GetResponseAsync(prompt);
            return Ok(response);
        }
    }
}