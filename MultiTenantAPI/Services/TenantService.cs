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

        public async Task<bool> CreateTenant(TenantCreateRequest request)
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
                    return false;
                }

                Console.WriteLine("SUCCESS : Database Created");

              
                Console.WriteLine("STEP 2 : Copying Folder");

                Console.WriteLine("SUCCESS : Folder Copied");

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

                bool containerCreated =
                    await _dockerService.CreateContainer(
                        request.SubDomain,
                        port);

                Console.WriteLine($"Docker Result : {containerCreated}");

                if (!containerCreated)
                {
                    Console.WriteLine("FAILED : Docker");
                    return false;
                }

                Console.WriteLine("SUCCESS : Docker Created");


                await _folderService.CopyFolder(request.SubDomain);



                // STEP 5

                Console.WriteLine("STEP 5 : Plesk Creating");

                bool pleskCreated =
                    await _pleskService.CreateSubDomain(
                        request.SubDomain);

                Console.WriteLine($"Plesk Result : {pleskCreated}");

                if (!pleskCreated)
                {
                    Console.WriteLine("FAILED : Plesk");
                    return false;
                }

                Console.WriteLine("SUCCESS : Plesk Created");
                Console.WriteLine("========== TENANT CREATED ==========");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION:");
                Console.WriteLine(ex.ToString());

                return false;
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