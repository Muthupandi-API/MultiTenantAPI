namespace MultiTenantAPI.Models
{
    public class TenantCreationResult
    {
        public bool Success { get; set; }

        // Short machine-friendly reason
        public string Message { get; set; }

        // Optional detailed text for debugging (stack traces, inner errors)
        public string Details { get; set; }
    }
}
