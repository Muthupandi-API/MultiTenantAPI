


using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public interface ITenantService
    {
        Task<bool> CreateTenant(TenantCreateRequest request);

        TenantConfig LoadConfig(string tenantId);
    }
}