using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public interface ITenantService
    {
        Task Create(TenantCreateRequest request);
    }
}