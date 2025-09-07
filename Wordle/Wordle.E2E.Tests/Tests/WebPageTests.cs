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
}