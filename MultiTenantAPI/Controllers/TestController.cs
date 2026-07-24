using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Data;
using MultiTenantAPI.Services;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ApplicationDbContext _context;


        public TestController(
            ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Host = HttpContext.Request.Host.Host,
                TenantId = HttpContext.Items["TenantId"],
                Connection = HttpContext.Items["ConnectionString"]
            });
        }



        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }

        [HttpGet("header")]
        public IActionResult Header(
            [FromHeader(Name = "X-Tenant")] string tenant)
        {
            return Ok(new
            {
                Header = tenant,
                Host = HttpContext.Request.Host.Host,
                TenantId = HttpContext.Items["TenantId"]
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