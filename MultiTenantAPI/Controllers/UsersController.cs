using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Data;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
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

        [HttpGet("users")]
        public IActionResult Users(
    [FromHeader(Name = "X-Tenant")] string tenant)
        {
            var users = _context.Users.ToList();

            return Ok(users);
        }



    }
}