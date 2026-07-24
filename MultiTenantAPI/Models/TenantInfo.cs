

namespace MultiTenantAPI.Models
{
    public class TenantInfo
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string SubDomain { get; set; }

        public string DatabaseName { get; set; }

        public string ConnectionString { get; set; }

        public string AdminEmail { get; set; }

        public int Port { get; set; }
    }
}