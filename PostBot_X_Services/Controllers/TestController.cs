using Microsoft.AspNetCore.Http;
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
    }
}
