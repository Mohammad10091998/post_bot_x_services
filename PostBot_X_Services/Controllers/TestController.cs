﻿using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

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

        [HttpPost("automated/write")]
        public async Task<IActionResult> RunAutomatedWriteTest(APITestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); 
                }
                var response = await _testService.RunAutomatedWriteTestsAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("automated/read")]
        public async Task<IActionResult> RunAutomatedReadTestsAsync(APITestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Automatically returns validation errors
                }
                var response = await _testService.RunAutomatedReadTestsAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("manual/write")]
        public async Task<IActionResult> RunManualWriteTestsAsync(APITestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Automatically returns validation errors
                }
                var response = await _testService.RunManualWriteTestsAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("manual/read")]
        public async Task<IActionResult> RunManualReadTestsAsync(APITestRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Automatically returns validation errors
                }
                var response = await _testService.RunManualReadTestsAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
