using Microsoft.AspNetCore.Mvc;
using ServiceA.Models;
using ServiceB.Models;
using System.Text.Json;

namespace ServiceB.Client
{
    public class StatusServiceClient
    {
        private readonly HttpClient _httpClient;

        public StatusServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("apiKey", "Akey");
        }

        public async Task<UnitStatusModel> GetStatusesAsync(int Id)
        {
            
            var response = await _httpClient.GetAsync($"http://localhost:5250/api/status/?Id={Id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UnitStatusModel>(content);
        }
    }
}
