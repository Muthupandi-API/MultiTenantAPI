namespace MultiTenantAPI.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var host = context.Request.Host.Host;

            string tenant = "default";

            if (!string.IsNullOrEmpty(host))
            {
                var parts = host.Split('.');

                if (parts.Length > 0)
                {
                    tenant = parts[0];
                }
            }

            context.Items["TenantId"] = tenant;

            Console.WriteLine($"Host = {host}");
            Console.WriteLine($"Tenant = {tenant}");

            await _next(context);
        }

        
    }
}