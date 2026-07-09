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

            var tenant = host.Split('.')[0];

            context.Items["Tenant"] = tenant;


            await _next(context);
        }
    }
}