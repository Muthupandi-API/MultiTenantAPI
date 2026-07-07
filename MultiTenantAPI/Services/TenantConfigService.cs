using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using MultiTenantAPI.Models;

namespace MultiTenantAPI.Services
{
    public class TenantConfigService
    {
        public TenantConfig LoadConfig(string tenantId)
        {
            var filePath = Path.Combine("Configs", $"tenant-{tenantId}.yaml");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Tenant config not found");
            }

            var yaml = File.ReadAllText(filePath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var config = deserializer.Deserialize<TenantConfig>(yaml);

            return config;
        }
    }
}