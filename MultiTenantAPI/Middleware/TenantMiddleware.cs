//namespace MultiTenantAPI.Middleware
//{
//    public class TenantMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public TenantMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            var host = context.Request.Host.Host;

//            string tenant = "default";

//            if (!string.IsNullOrEmpty(host))
//            {
//                var parts = host.Split('.');

//                if (parts.Length > 0)
//                {
//                    tenant = parts[0];
//                }
//            }

//            context.Items["TenantId"] = tenant;

//            Console.WriteLine($"Host = {host}");
//            Console.WriteLine($"Tenant = {tenant}");

//            await _next(context);
//        }


//    }
//}
using MultiTenantAPI.Repository;

namespace MultiTenantAPI.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            ITenantRepository repository)
        {
            // Skip System APIs (No Tenant Required)
            if (context.Request.Path.StartsWithSegments("/swagger") ||
     context.Request.Path.StartsWithSegments("/api/Tenant/create") ||
     context.Request.Path.StartsWithSegments("/api/folder"))
            {
                await _next(context);
                return;
            }

            string host = context.Request.Host.Host;

            // First check X-Tenant header (Local Testing)
            string tenant =
                context.Request.Headers["X-Tenant"]
                .FirstOrDefault() ?? "";

            // If no header, get tenant from subdomain
            if (string.IsNullOrWhiteSpace(tenant))
            {
                var parts = host.Split('.');

                if (parts.Length > 0)
                {
                    tenant = parts[0];
                }
            }

            Console.WriteLine("========== TENANT MIDDLEWARE ==========");
            Console.WriteLine($"Request Path     : {context.Request.Path}");
            Console.WriteLine($"Host             : {host}");
            Console.WriteLine($"Header X-Tenant  : {context.Request.Headers["X-Tenant"]}");
            Console.WriteLine($"Resolved Tenant  : {tenant}");

            // Get tenant details from Master DB
            var tenantInfo = await repository.GetTenantBySubDomain(tenant);

            Console.WriteLine($"Tenant Found     : {tenantInfo != null}");

            if (tenantInfo == null)
            {
                Console.WriteLine("Tenant Not Found");

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Tenant not found.");

                return;
            }

            Console.WriteLine($"Database         : {tenantInfo.DatabaseName}");

            // Store tenant information for DbContext
            context.Items["TenantId"] = tenantInfo.SubDomain;
            context.Items["ConnectionString"] = tenantInfo.ConnectionString;

            await _next(context);
        }
    }
}