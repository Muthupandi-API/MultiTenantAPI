using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MultiTenantAPI.Models;
using MultiTenantAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly TenantConfigService _tenantService;

    public TenantController(IConfiguration configuration, TenantConfigService tenantService)
    {
        _configuration = configuration;
        _tenantService = tenantService;
    }

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
            return StatusCode(500, new
            {
                Message = "Error loading tenant config",
                Error = ex.Message
            });
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTenant([FromBody] TenantCreateRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.DatabaseName))
        {
            return BadRequest(new { Message = "Invalid request" });
        }

        var connectionString = _configuration.GetConnectionString("MasterConnection");

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            string sql = $"CREATE DATABASE [{request.DatabaseName}]";

            using SqlCommand command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                Message = "Database Created Successfully",
                Database = request.DatabaseName
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "Error creating database",
                Error = ex.Message
            });
        }
    }
}