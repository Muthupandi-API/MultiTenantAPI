namespace MultiTenantAPI.Services
{
    public class FolderService
    {
        private readonly HttpClient _client;

        public FolderService(HttpClient client)
        {
            _client = client;
        }

        public async Task CopyFolder(string tenant)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(
                    "https://demo1.jailscorp.com/api/folder/copy",
                    new
                    {
                        TenantName = tenant
                    });

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();

                    throw new Exception(
                        $"Folder Copy Failed\n" +
                        $"Status Code : {(int)response.StatusCode}\n" +
                        $"Response : {error}"
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"FolderService Error : {ex.Message}",
                    ex
                );
            }
        }
    }

}
