
using Microsoft.AspNetCore.Mvc;
using MultiTenantAPI.Docker;
using MultiTenantAPI.Models;
using MultiTenantAPI.Repository;
using MultiTenantAPI.Services;

namespace MultiTenantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly FolderService _folderService;

        public TenantController(ITenantService tenantService,
            FolderService folderService)
        {
            _tenantService = tenantService;
        }

        // Check Current Tenant

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Host = HttpContext.Request.Host.Host,
                Tenant = HttpContext.Items["TenantId"]?.ToString()
            });
        }

        // Get Tenant Configuration
        [HttpGet("get-config/{tenantId}")]

        public IActionResult GetConfig(string tenantId)
        {
            try
            {
                var config = _tenantService.LoadConfig(tenantId);
                return Ok(config);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    Message = "Error loading tenant configuration.",
                    Error = ex.Message
                });
            }
        }




        // Create New Tenant

        [HttpPost("create")]
        public async Task<IActionResult> CreateTenant([FromBody] TenantCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    Message = "Request cannot be null."
                });
            }



            if (string.IsNullOrWhiteSpace(request.CompanyName) ||
                string.IsNullOrWhiteSpace(request.SubDomain) ||
                string.IsNullOrWhiteSpace(request.DatabaseName) ||
                string.IsNullOrWhiteSpace(request.AdminEmail))
            {
                return BadRequest(new
                {
                    Message = "All fields are required."
                });
            }

            try
            {
                Console.WriteLine("========== CREATE TENANT ==========");
                Console.WriteLine($"Company   : {request.CompanyName}");
                Console.WriteLine($"SubDomain : {request.SubDomain}");
                Console.WriteLine($"Database  : {request.DatabaseName}");
                Console.WriteLine($"Email     : {request.AdminEmail}");

                var result = await _tenantService.CreateTenant(request);

                if (result == null || !result.Success)
                {
                    Console.WriteLine($"Tenant creation failed: {result?.Message}");

                    return StatusCode(500, new
                    {
                        Message = "Tenant creation failed.",
                        Reason = result?.Message,
                        Details = result?.Details
                    });
                }

                Console.WriteLine("Tenant created successfully.");
                //await _folderService.CopyFolder("client1");
                return Ok(new
                {
                    Message = "Tenant created successfully.",
                    CompanyName = request.CompanyName,
                    SubDomain = request.SubDomain,
                    Database = request.DatabaseName,
                    AdminEmail = request.AdminEmail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, new
                {
                    Message = "An error occurred while creating the tenant.",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace // Debug only
                });
            }
        }
    }
}