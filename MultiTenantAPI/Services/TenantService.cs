using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public class TenantService : ITenantService
    {
        public async Task Create(TenantCreateRequest request)
        {
            // இங்கு Database Save Logic வரும்

            Console.WriteLine(request.CompanyName);
            Console.WriteLine(request.SubDomain);
            Console.WriteLine(request.DatabaseName);
            Console.WriteLine(request.AdminEmail);

            await Task.CompletedTask;
        }
    }
}