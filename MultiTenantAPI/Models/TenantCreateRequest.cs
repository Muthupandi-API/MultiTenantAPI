namespace MultiTenantAPI.Models
{
    public class TenantCreateRequest
    {
        public string CompanyName { get; set; }

        public string SubDomain { get; set; }

        public string DatabaseName { get; set; }

        public string AdminEmail { get; set; }
    }
}