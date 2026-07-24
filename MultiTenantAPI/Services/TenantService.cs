using MultiTenantAPI.Docker;
using MultiTenantAPI.Models;
using MultiTenantAPI.Repository;

namespace MultiTenantAPI.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IDockerService _dockerService;
        private readonly IPleskService _pleskService;
        private readonly FolderService _folderService;

        public TenantService(
            ITenantRepository repository,
            IConfiguration configuration,
            IDockerService dockerService,
            IPleskService pleskService,
            FolderService folderService    )
        {
            _repository = repository;
            _configuration = configuration;
            _dockerService = dockerService;
            _pleskService = pleskService;
            _folderService = folderService;
        }

        public async Task<TenantCreationResult> CreateTenant(TenantCreateRequest request)
        {
            try
            {
                Console.WriteLine("========== TENANT CREATION ==========");

                // STEP 1
                Console.WriteLine("STEP 1 : Database Creating");

                bool databaseCreated =
                    await _repository.CreateDatabase(request.DatabaseName);



                Console.WriteLine($"Database Result : {databaseCreated}");

                if (!databaseCreated)
                {
                    Console.WriteLine("FAILED : Database");
                    return new TenantCreationResult
                    {
                        Success = false,
                        Message = "Database creation failed",
                        Details = "Repository failed to create database"
                    };
                }

                Console.WriteLine("SUCCESS : Database Created");

              
                Console.WriteLine("STEP 2 : Copying Folder");

                // Folder copy will be performed after tenant creation completes.
                // Do not perform it here to avoid blocking early in the creation flow.
                // STEP 2


                Console.WriteLine("STEP 2 : Getting Port");

                int port = await _repository.GetNextPort();

                Console.WriteLine($"Port : {port}");


                // STEP 3
                Console.WriteLine("STEP 3 : Saving Tenant");

                await _repository.SaveTenant(request, port);

                Console.WriteLine("SUCCESS : Tenant Saved");


                // STEP 4

                Console.WriteLine("STEP 4 : Docker Creating");

                var dockerResult =
                    await _dockerService.CreateContainer(
                        request.SubDomain,
                        port);

                Console.WriteLine($"Docker Result : Success={dockerResult.Success} ExitCode={dockerResult.ExitCode}");

                if (!dockerResult.Success)
                {
                    Console.WriteLine("FAILED : Docker");
                    return new TenantCreationResult
                    {
                        Success = false,
                        Message = "Docker container creation failed",
                        Details = $"ExitCode={dockerResult.ExitCode}; Error={dockerResult.Error}; Output={dockerResult.Output}; Command={dockerResult.Command}"
                    };
                }

                Console.WriteLine("SUCCESS : Docker Created");




                // STEP 5

                Console.WriteLine("STEP 5 : Plesk Creating");

                bool pleskCreated =
                    await _pleskService.CreateSubDomain(
                        request.SubDomain);

                Console.WriteLine($"Plesk Result : {pleskCreated}");

                if (!pleskCreated)
                {
                    Console.WriteLine("FAILED : Plesk");
                    return new TenantCreationResult
                    {
                        Success = false,
                        Message = "Plesk subdomain creation failed",
                        Details = "PleskService failed to create subdomain"
                    };
                }

                Console.WriteLine("SUCCESS : Plesk Created");
                Console.WriteLine("========== TENANT CREATED ==========");

                // Wait 2 seconds, then perform the folder copy as a final post-creation step.
                try
                {
                    await Task.Delay(2000);
                    await _folderService.CopyFolder(request.SubDomain);
                    Console.WriteLine("SUCCESS : Folder Copied (post-create)");
                }
                catch (Exception ex)
                {
                    // Log error but do not mark overall creation as failed since tenant is created.
                    Console.WriteLine($"WARNING : Post-create folder copy failed: {ex}");
                }

                return new TenantCreationResult
                {
                    Success = true,
                    Message = "Tenant created successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== EXCEPTION ==========");
                Console.WriteLine(ex.ToString());

                return new TenantCreationResult
                {
                    Success = false,
                    Message = "Unhandled exception during tenant creation",
                    Details = ex.ToString()
                };
            }
        }

        public TenantConfig LoadConfig(string tenantId)
        {
            return new TenantConfig
            {
                Tenant = tenantId,
                Database = new DatabaseConfig
                {
                    ConnectionString = _configuration.GetConnectionString("DefaultConnection")
                }
            };
        }
    }
}