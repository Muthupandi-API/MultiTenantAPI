using System.Text;

namespace MultiTenantAPI.Services
{
    public class PleskService : IPleskService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;


      

        public PleskService(IConfiguration configuration)
        {
            _configuration = configuration;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                (message, cert, chain, error) => true
            };


            _client = new HttpClient(handler);


            _client.DefaultRequestHeaders.Add(
                "KEY",
                _configuration["Plesk:XmlSecretKey"]
            );
        }




        public async Task<bool> CreateSubDomain(string subDomain)
        {
            try
            {
                var url = _configuration["Plesk:Url"] + "/enterprise/control/agent.php";

                var parentDomain = _configuration["Plesk:ParentDomain"];

                string xml = $@"
<packet version='1.6.9.1'>
  <subdomain>
    <add>
      <parent>{parentDomain}</parent>
      <name>{subDomain}</name>
    </add>
  </subdomain>
</packet>";

                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                var response = await _client.PostAsync(url, content);

                string result = await response.Content.ReadAsStringAsync();

                // Print full response
                Console.WriteLine(result);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"HTTP {(int)response.StatusCode}\n{result}");
                }

                if (!result.Contains("<status>ok</status>"))
                {
                    throw new Exception(
                        $"Plesk API Error:\n{result}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"CreateSubDomain Failed: {ex.Message}",
                    ex);
            }
        }



    }
}