using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Wordle.E2E.Tests.Framework;

[Collection("E2ETest")]
public abstract class E2ETestBase
{
    protected readonly HttpClient HttpClient;
    protected readonly RealServerTestFixture ServerFixture;

    protected E2ETestBase(RealServerTestFixture serverFixture)
    {
        ServerFixture = serverFixture;
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri($"{serverFixture.ApiBaseUrl}/api/")
        };
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await HttpClient.GetAsync(endpoint);
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T payload)
    {
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        return await HttpClient.PostAsync(endpoint, content);
    }
}