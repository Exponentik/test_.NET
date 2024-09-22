using ServiceA.Models;
using System.Text.Json;

namespace ServiceA.Client
{
    public class UnitStatusService
    {
        private readonly HttpClient _httpClient;

        public UnitStatusService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UnitStatusModel>> GetStatusesAsync()
        {
            var response = await _httpClient.GetAsync("api/status");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<UnitStatusModel>>(content);
        }
    }
}
