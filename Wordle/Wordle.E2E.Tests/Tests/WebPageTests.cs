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
        var response = await Page.GotoAsync(ServerFixture.WebBaseUrl);
        
        response.Should().NotBeNull();
        response!.Ok.Should().BeTrue();
        response.Status.Should().Be(200);
    }

    [Fact]
    public async Task WebPage_ShouldDisplay_StatusPageWithApiHealth()
    {
        await Page.GotoAsync(ServerFixture.WebBaseUrl);
        
        // Check page title
        var title = await Page.TitleAsync();
        title.Should().Contain("Wordle");
        
        // Check for main heading
        var heading = await Page.TextContentAsync("h1");
        heading.Should().Contain("Wordle Web Status");
        
        // Check for API status elements
        var apiStatusLabel = await Page.TextContentAsync("text=API Status:");
        apiStatusLabel.Should().NotBeNull();
        
        // Check for status value (OK or Error)
        var statusElement = await Page.QuerySelectorAsync("p:has-text('API Status:') strong");
        statusElement.Should().NotBeNull();
        var statusText = await statusElement!.TextContentAsync();
        statusText.Should().BeOneOf("OK", "Error");
        
        // Check for Last Checked timestamp
        var lastCheckedElement = await Page.QuerySelectorAsync("text=Last Checked:");
        lastCheckedElement.Should().NotBeNull();
    }

    [Fact]
    public async Task WebPage_ShouldHaveCorrectStructure()
    {
        await Page.GotoAsync(ServerFixture.WebBaseUrl);
        
        // Check for proper HTML structure
        var htmlElement = await Page.QuerySelectorAsync("html");
        htmlElement.Should().NotBeNull();
        
        var bodyElement = await Page.QuerySelectorAsync("body");
        bodyElement.Should().NotBeNull();
        
        // Check for container div
        var containerElement = await Page.QuerySelectorAsync(".container");
        containerElement.Should().NotBeNull();
        
        // Verify the page content is visible
        var isVisible = await Page.IsVisibleAsync("h1");
        isVisible.Should().BeTrue();
    }
}