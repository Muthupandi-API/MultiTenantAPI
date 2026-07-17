using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Models;
using MultiTenantAPI.Services;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {

        private readonly ITenantService _tenantService;


        public TenantController(
            ITenantService tenantService)
        {
            _tenantService = tenantService;
        }



        // Check Tenant Middleware
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Host = HttpContext.Request.Host.Host,

                Tenant =
                HttpContext.Items["TenantId"]?.ToString()
            });
        }




        // Get Tenant Configuration
        [HttpGet("get-config/{tenantId}")]
        public IActionResult GetConfig(string tenantId)
        {
            try
            {

                // TenantService  config load 
                var config =
                    _tenantService.LoadConfig(tenantId);


                return Ok(config);

            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Message = "Error loading tenant config",

                    Error = ex.Message
                });

            }
        }





        // Create Tenant Database
        [HttpPost("create")]
        public async Task<IActionResult> CreateTenant(
            [FromBody] TenantCreateRequest request)
        {

            if (request == null ||
               string.IsNullOrWhiteSpace(request.DatabaseName))
            {
                return BadRequest(new
                {
                    Message = "Invalid request"
                });
            }



            try
            {

                // Save Tenant details
                await _tenantService.Create(request);



                // Create Tenant Database
                await _tenantService.CreateDatabase(
                    request.DatabaseName);



                return Ok(new
                {
                    Message =
                    "Tenant Database Created Successfully",

                    Database =
                    request.DatabaseName
                });

            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Message =
                    "Error creating tenant database",

                    Error =
                    ex.Message
                });

            }

        }

    }
}