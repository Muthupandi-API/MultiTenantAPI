//using Microsoft.AspNetCore.Mvc;

//namespace MultiTenantAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TestController : ControllerBase
//    {
//        [HttpGet("tenant")]
//        public IActionResult GetTenant()
//        {
//            return Ok(new
//            {
//                Host = HttpContext.Request.Host.Host,
//                Tenant = HttpContext.Items["TenantId"]?.ToString()
//            });
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var tenant = HttpContext.Items["TenantId"]?.ToString();
        //    var host = HttpContext.Request.Host.Host;

        //    return Ok(new
        //    {
        //        Host = host,
        //        Tenant = tenant,
        //        Message = "Tenant detected successfully"
        //    });
        //}

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
    }
}