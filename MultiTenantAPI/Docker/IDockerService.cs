namespace MultiTenantAPI.Docker
{
    public interface IDockerService
    {
    

        Task<bool> CreateContainer(string subDomain, int port);
    }
}

