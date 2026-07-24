namespace MultiTenantAPI.Services
{
    public interface IConnectionProvider
    {
        string GetConnectionString();
    }
}