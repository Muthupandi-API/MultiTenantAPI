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

                var masterConnection =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(masterConnection);

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
                Console.WriteLine(ex);

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

                var masterConnection =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(masterConnection);

                await connection.OpenAsync();

                Console.WriteLine($"Connected Database : {connection.Database}");

                var builder = new SqlConnectionStringBuilder(masterConnection);
                builder.InitialCatalog = request.DatabaseName;

                string tenantConnectionString = builder.ConnectionString;

                string sql = @"
INSERT INTO Tenants
(
    CompanyName,
    SubDomain,
    DatabaseName,
    AdminEmail,
    Port,
    ConnectionString
)
VALUES
(
    @CompanyName,
    @SubDomain,
    @DatabaseName,
    @AdminEmail,
    @Port,
    @ConnectionString
)";

                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@CompanyName", request.CompanyName);
                command.Parameters.AddWithValue("@SubDomain", request.SubDomain);
                command.Parameters.AddWithValue("@DatabaseName", request.DatabaseName);
                command.Parameters.AddWithValue("@AdminEmail", request.AdminEmail);
                command.Parameters.AddWithValue("@Port", port);
                command.Parameters.AddWithValue("@ConnectionString", tenantConnectionString);

                Console.WriteLine("Executing INSERT...");

                int rows = await command.ExecuteNonQueryAsync();

                Console.WriteLine($"Rows Inserted = {rows}");

                Console.WriteLine("Tenant saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== SAVE TENANT ERROR ==========");
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

                var masterConnection =
                    _configuration.GetConnectionString("MasterConnection");

                using SqlConnection connection =
                    new SqlConnection(masterConnection);

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
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<TenantInfo?> GetTenantBySubDomain(string subDomain)
        {
            var masterConnection =
                _configuration.GetConnectionString("MasterConnection");

            using SqlConnection connection =
                new SqlConnection(masterConnection);

            await connection.OpenAsync();

            Console.WriteLine($"Searching Tenant = {subDomain}");
            Console.WriteLine($"Database = {connection.Database}");

            Console.WriteLine($"Searching SubDomain = '{subDomain}'");

            string sql = @"
SELECT
    CompanyName,
    SubDomain,
    DatabaseName,
    ConnectionString,
    AdminEmail,
    Port
FROM Tenants
WHERE SubDomain = @SubDomain";

            using SqlCommand command =
                new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@SubDomain", subDomain);

            using SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new TenantInfo
            {
                CompanyName = reader["CompanyName"]?.ToString() ?? "",
                SubDomain = reader["SubDomain"]?.ToString() ?? "",
                DatabaseName = reader["DatabaseName"]?.ToString() ?? "",
                ConnectionString = reader["ConnectionString"]?.ToString() ?? "",
                AdminEmail = reader["AdminEmail"]?.ToString() ?? "",
                Port = reader["Port"] != DBNull.Value
                            ? Convert.ToInt32(reader["Port"])
                            : 0
            };
        }

    }
}

