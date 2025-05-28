using INDIGIPLUS.Client.Services.Interfaces;

namespace INDIGIPLUS.Client.Services
{
    public class TestConnectionClientService : ITestConnectionClientService
    {
        private readonly HttpClient _httpClient;

        public TestConnectionClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TestApiConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/testconnection/test");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"API responded with status: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Connection failed: {ex.Message}";
            }
        }
    }
}
