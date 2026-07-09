using MultiTenantAPI.Models;

namespace MultiTenantAPI.Repository
{
    public interface ITenantRepository
    {
        Task<bool> CreateDatabase(string databaseName);

        TenantConfig LoadConfig(string tenantId);
    }
}