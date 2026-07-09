
using MultiTenantAPI.Models;
using MultiTenantAPI.Repository;           

namespace MultiTenantAPI.Services
{
    public class TenantService : ITenantService
    {

        private readonly ITenantRepository _repository;
        private readonly IConfiguration _configuration;


        public TenantService(
            ITenantRepository repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }



        public async Task Create(
            TenantCreateRequest request)
        {
            Console.WriteLine(request.CompanyName);
            Console.WriteLine(request.SubDomain);
            Console.WriteLine(request.DatabaseName);
            Console.WriteLine(request.AdminEmail);

            await Task.CompletedTask;
        }



        public async Task<bool> CreateDatabase(
            string databaseName)
        {
            return await _repository
                .CreateDatabase(databaseName);
        }



        //  LoadConfig()
        public TenantConfig LoadConfig(string tenantId)
        {
            return new TenantConfig
            {
                Tenant = tenantId,

                Database = new DatabaseConfig
                {
                    ConnectionString =
                    _configuration
                    .GetConnectionString("DefaultConnection")
                }
            };
        }

    }
}