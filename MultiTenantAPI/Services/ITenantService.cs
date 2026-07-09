
using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public interface ITenantService
    {
        Task Create(TenantCreateRequest request);

        Task<bool> CreateDatabase(string databaseName);

        TenantConfig LoadConfig(string tenantId);
    }
}