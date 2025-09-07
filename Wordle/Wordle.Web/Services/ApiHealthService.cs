using System.Text.Json;
using Wordle.Web.Models;

namespace Wordle.Web.Services;

public class ApiHealthService : IApiHealthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ApiHealthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ApiStatusModel> GetApiStatusAsync()
    {
        var model = new ApiStatusModel { LastChecked = DateTime.UtcNow };

        try
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5000/api";
            var response = await _httpClient.GetAsync($"{apiBaseUrl}/system/health");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var healthResponse = JsonSerializer.Deserialize<HealthCheckResponse>(content, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                
                model.IsHealthy = healthResponse?.Ok == true;
            }
            else
            {
                model.IsHealthy = false;
            }
        }
        catch
        {
            model.IsHealthy = false;
        }

        return model;
    }

    private class HealthCheckResponse
    {
        public bool Ok { get; set; }
        public DateTime Timestamp { get; set; }
    }
}