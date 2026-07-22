using System.Diagnostics;

namespace MultiTenantAPI.Docker
{
    public class DockerService : IDockerService
    {
        public async Task<bool> CreateContainer(
            string subDomain,
            int port)
        {
            try
            {
                // Container Name
                string containerName = $"{subDomain}-api";


                // Docker Image
                string imageName =
                    "muthupandimathi/multitenantapi:36";


                // Host Port
                int hostPort = port;



                // Docker Command
                string command =
                    $"docker run -d " +
                    $"--name {containerName} " +
                    $"-p {hostPort}:8080 " +
                    $"{imageName}";



                ProcessStartInfo startInfo =
                    new ProcessStartInfo
                    {
                        FileName = "cmd.exe",

                        Arguments = "/c " + command,

                        RedirectStandardOutput = true,

                        RedirectStandardError = true,

                        UseShellExecute = false,

                        CreateNoWindow = true
                    };



                using Process process = new Process();

                process.StartInfo = startInfo;


                process.Start();



                string output =
                    await process.StandardOutput
                    .ReadToEndAsync();


                string error =
                    await process.StandardError
                    .ReadToEndAsync();



                process.WaitForExit();



                Console.WriteLine("Docker Output:");
                Console.WriteLine(output);



                if (process.ExitCode != 0)
                {
                    Console.WriteLine("Docker Error:");
                    Console.WriteLine(error);

                    Console.WriteLine("Command:");      // extra code
                    Console.WriteLine(command);

                    Console.WriteLine("Exit Code:");
                    Console.WriteLine(process.ExitCode);

                    Console.WriteLine("Output:");
                    Console.WriteLine(output);

                    Console.WriteLine("Error:");
                    Console.WriteLine(error);

                    return false;
                }



                Console.WriteLine(
                    "Docker Container Created Successfully.");


                Console.WriteLine(
                    $"Container Name : {containerName}");


                Console.WriteLine(
                    $"Image          : {imageName}");


                Console.WriteLine(
                    $"Port           : {hostPort}");



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Docker Exception: " + ex.Message);

                return false;
            }
        }
    }
}