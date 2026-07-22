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
                var url =
                _configuration["Plesk:Url"] +
                "/enterprise/control/agent.php";


                var parentDomain =
                _configuration["Plesk:ParentDomain"];


                string xml = $@"
<packet>
 <subdomain>
  <add>
   <parent>{parentDomain}</parent>
   <name>{subDomain}</name>
  </add>
 </subdomain>
</packet>";


                var content =
                new StringContent(
                    xml,
                    Encoding.UTF8,
                    "text/xml"
                );


                var response =
                await _client.PostAsync(
                    url,
                    content
                );


                var result =
                await response.Content.ReadAsStringAsync();


                Console.WriteLine(result);


                return result.Contains(
                    "<status>ok</status>"
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }





    }
}