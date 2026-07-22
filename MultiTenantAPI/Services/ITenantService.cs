


using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public interface ITenantService
    {
        Task<TenantCreationResult> CreateTenant(TenantCreateRequest request);

        TenantConfig LoadConfig(string tenantId);
    }
}