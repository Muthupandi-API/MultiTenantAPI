

using Microsoft.Data.SqlClient;
using MultiTenantAPI.Models;

namespace MultiTenantAPI.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IConfiguration _configuration;

        public TenantRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Create Tenant Database
        public async Task<bool> CreateDatabase(string databaseName)
        {
            try
            {
                Console.WriteLine("===== CREATE DATABASE =====");

                var connectionString =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(connectionString);

                await connection.OpenAsync();

                Console.WriteLine("Connected to SQL Server");

                // Check if database already exists
                string checkSql =
                    "SELECT COUNT(*) FROM sys.databases WHERE name = @DatabaseName";

                using SqlCommand checkCommand =
                    new SqlCommand(checkSql, connection);

                checkCommand.Parameters.AddWithValue(
                    "@DatabaseName",
                    databaseName);

                int count = Convert.ToInt32(
                    await checkCommand.ExecuteScalarAsync());

                if (count > 0)
                {
                    Console.WriteLine($"Database '{databaseName}' already exists.");
                    return true;
                }

                string createSql =
                    $"CREATE DATABASE [{databaseName}]";

                using SqlCommand createCommand =
                    new SqlCommand(createSql, connection);

                await createCommand.ExecuteNonQueryAsync();

                Console.WriteLine($"Database '{databaseName}' created successfully.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateDatabase Error:");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }




        // Save Tenant Details
        public async Task SaveTenant(
            TenantCreateRequest request,
            int port)
        {
            try
            {
                Console.WriteLine("===== SAVE TENANT =====");

                var connectionString =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(connectionString);

                await connection.OpenAsync();

                string sql = @"
INSERT INTO Tenants
(
    CompanyName,
    SubDomain,
    DatabaseName,
    AdminEmail,
    Port
)
VALUES
(
    @CompanyName,
    @SubDomain,
    @DatabaseName,
    @AdminEmail,
    @Port
)";

                using SqlCommand command =
                    new SqlCommand(sql, connection);

                command.Parameters.AddWithValue(
                    "@CompanyName",
                    request.CompanyName);

                command.Parameters.AddWithValue(
                    "@SubDomain",
                    request.SubDomain);

                command.Parameters.AddWithValue(
                    "@DatabaseName",
                    request.DatabaseName);

                command.Parameters.AddWithValue(
                    "@AdminEmail",
                    request.AdminEmail);

                command.Parameters.AddWithValue(
                    "@Port",
                    port);

                await command.ExecuteNonQueryAsync();

                Console.WriteLine("Tenant saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveTenant Error:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Get Next Available Port
        public async Task<int> GetNextPort()
        {
            try
            {
                Console.WriteLine("===== GET NEXT PORT =====");

                var connectionString =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(connectionString);


                await connection.OpenAsync();


                string sql =
                    "SELECT ISNULL(MAX(Port), 5000) + 1 FROM Tenants";

                using SqlCommand command =
                    new SqlCommand(sql, connection);

                object result =
                    await command.ExecuteScalarAsync();

                int port = Convert.ToInt32(result);

                Console.WriteLine($"Next Port : {port}");

                return port;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetNextPort Error:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}