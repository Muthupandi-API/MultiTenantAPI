namespace MultiTenantAPI.Models
{
    public class TenantConfig
    {
        public string Tenant { get; set; }

        public DatabaseConfig Database { get; set; }
    }

    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
    }
}