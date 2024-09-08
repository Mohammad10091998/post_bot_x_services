using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> RunAutomatedWriteTest(APITestRequestModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _testService.RunAutomatedWriteTestsAsync(model, cancellationToken);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Client Closed Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("automated/read")]
        public async Task<IActionResult> RunAutomatedReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _testService.RunAutomatedReadTestsAsync(model, cancellationToken);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Client Closed Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("manual/write")]
        public async Task<IActionResult> RunManualWriteTestsAsync(APITestRequestModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _testService.RunManualWriteTestsAsync(model, cancellationToken);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Client Closed Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("manual/read")]
        public async Task<IActionResult> RunManualReadTestsAsync(APITestRequestModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _testService.RunManualReadTestsAsync(model, cancellationToken);
                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Client Closed Request");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
