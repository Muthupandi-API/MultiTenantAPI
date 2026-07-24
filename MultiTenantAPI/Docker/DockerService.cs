
using System.Diagnostics;

namespace MultiTenantAPI.Docker
{
    public class DockerService : IDockerService
    {
        public async Task<DockerResult> CreateContainer(
            string subDomain,
            int port)
        {
            try
            {
                // Container Name
                string containerName = $"{subDomain}-api";

                // Docker Image
                string imageName = "muthupandimathi/multitenantapi:36";

                // Host Port
                int hostPort = port;

                // Full Docker Path
                string dockerExe =
                    @"C:\Program Files\Docker\Docker\resources\bin\docker.exe";

                // Docker Arguments
                string arguments =
                    $"run -d --name {containerName} -p {hostPort}:8080 {imageName}";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = dockerExe,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();

                string output =
                    await process.StandardOutput.ReadToEndAsync();

                string error =
                    await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                var result = new DockerResult
                {
                    Success = process.ExitCode == 0,
                    ExitCode = process.ExitCode,
                    Output = output,
                    Error = error,
                    Command = $"{dockerExe} {arguments}"
                };

                if (!result.Success)
                {
                    Console.WriteLine("========== DOCKER ERROR ==========");
                    Console.WriteLine($"ExitCode : {result.ExitCode}");
                    Console.WriteLine($"Command  : {result.Command}");
                    Console.WriteLine($"Output   : {result.Output}");
                    Console.WriteLine($"Error    : {result.Error}");
                }
                else
                {
                    Console.WriteLine("========== DOCKER SUCCESS ==========");
                    Console.WriteLine($"Container : {containerName}");
                    Console.WriteLine($"Image     : {imageName}");
                    Console.WriteLine($"Port      : {hostPort}");
                    Console.WriteLine($"Output    : {output}");
                }

                return result;
            }
            catch (Exception ex)
            {
                return new DockerResult
                {
                    Success = false,
                    ExitCode = -1,
                    Output = string.Empty,
                    Error = ex.ToString(),
                    Command = string.Empty
                };
            }
        }
    }
}