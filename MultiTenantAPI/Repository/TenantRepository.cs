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



        public async Task<bool> CreateDatabase(string databaseName)
        {

            var connectionString =
                _configuration
                .GetConnectionString("MasterConnection");


            using SqlConnection connection =
                new SqlConnection(connectionString);


            await connection.OpenAsync();


            string sql =
                $"CREATE DATABASE [{databaseName}]";


            using SqlCommand command =
                new SqlCommand(sql, connection);


            await command.ExecuteNonQueryAsync();


            return true;
        }





        public TenantConfig LoadConfig(string tenantId)
        {

            return new TenantConfig
            {
                Tenant = tenantId,

                Database = new DatabaseConfig
                {
                    ConnectionString =
                    _configuration
                    .GetConnectionString("DefaultConnection")
                }
            };

        }

    }
}