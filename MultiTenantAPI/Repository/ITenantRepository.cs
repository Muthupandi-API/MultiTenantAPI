

using MultiTenantAPI.Models;

namespace MultiTenantAPI.Repository
{
    public interface ITenantRepository
    {
        Task<bool> CreateDatabase(string databaseName);

        Task SaveTenant(
            TenantCreateRequest request,
            int port);

        Task<int> GetNextPort();

        Task<TenantInfo?> GetTenantBySubDomain(string subDomain);


    }
}