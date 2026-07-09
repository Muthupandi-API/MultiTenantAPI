using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("tenant")]
        public IActionResult GetTenant()
        {
            return Ok(new
            {
                Host = HttpContext.Request.Host.Host,
                Tenant = HttpContext.Items["TenantId"]?.ToString()
            });
        }
    }
}