

using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Services;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Host = HttpContext.Request.Host.Host,
                TenantId = HttpContext.Items["TenantId"],
                AllItems = HttpContext.Items.Keys
            });
        }


        [HttpPost("test-plesk")]
        public async Task<IActionResult> Test(
        [FromServices] IPleskService plesk)
        {
            var result =
            await plesk.CreateSubDomain("customer101");

            return Ok(result);
        }

    }
}