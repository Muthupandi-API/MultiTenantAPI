namespace MultiTenantAPI.Docker
{
    public interface IDockerService
    {
        Task<DockerResult> CreateContainer(string subDomain, int port);
    }
}

