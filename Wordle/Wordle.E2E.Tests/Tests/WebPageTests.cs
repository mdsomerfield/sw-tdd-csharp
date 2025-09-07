using FluentAssertions;
using Wordle.E2E.Tests.Framework;

namespace Wordle.E2E.Tests;

public class WebPageE2ETests : E2ETestBase
{
    public WebPageE2ETests(RealServerTestFixture serverFixture) : base(serverFixture)
    {
    }

    [Fact]
    public async Task WebPage_ShouldReturn_200OK()
    {
        using var webClient = new HttpClient();
        
        var response = await webClient.GetAsync(ServerFixture.WebBaseUrl);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task WebPage_ShouldDisplay_StatusPageWithApiHealth()
    {
        using var webClient = new HttpClient();
        
        var response = await webClient.GetAsync(ServerFixture.WebBaseUrl);
        var content = await response.Content.ReadAsStringAsync();
        
        response.IsSuccessStatusCode.Should().BeTrue();
        content.Should().Contain("Wordle Web Status");
        content.Should().Contain("API Status:");
        content.Should().Contain("Last Checked:");
        content.Should().MatchRegex("API Status: <strong>(OK|Error)</strong>");
    }
}