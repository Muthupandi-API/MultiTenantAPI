using Microsoft.AspNetCore.Http;

namespace MultiTenantAPI.Services
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ConnectionProvider(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            // Connection String set by TenantMiddleware
            var connectionString =
                _httpContextAccessor.HttpContext?
                .Items["ConnectionString"]?.ToString();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }

            // Fallback to DefaultConnection
            return _configuration.GetConnectionString("DefaultConnection")
                   ?? throw new Exception("DefaultConnection not found.");
        }



    }
}