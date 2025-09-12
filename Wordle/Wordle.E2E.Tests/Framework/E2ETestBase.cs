using System.Text.Json;
using FluentAssertions;
using Microsoft.Playwright;
using Xunit;

namespace Wordle.E2E.Tests.Framework;

[Collection("E2ETest")]
public abstract class E2ETestBase : IAsyncLifetime
{
    protected readonly HttpClient HttpClient;
    protected readonly RealServerTestFixture ServerFixture;
    
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    protected IPage Page { get; private set; } = null!;

    protected E2ETestBase(RealServerTestFixture serverFixture)
    {
        ServerFixture = serverFixture;
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri($"{serverFixture.ApiBaseUrl}")
        };
    }

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        Page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
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