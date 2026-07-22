namespace MultiTenantAPI.Services
{
    public interface IPleskService
    {
        Task<bool> CreateSubDomain(string subDomain);
    }
}

