namespace MultiTenantAPI.Docker
{
    public class DockerResult
    {
        public bool Success { get; set; }

        public int ExitCode { get; set; }

        public string Output { get; set; }

        public string Error { get; set; }

        public string Command { get; set; }
    }
}
