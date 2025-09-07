using FluentAssertions;
using Wordle.E2E.Tests.Framework;

namespace Wordle.E2E.Tests;

public class ServerHealthE2ETests : E2ETestBase
{
    public ServerHealthE2ETests(RealServerTestFixture serverFixture) : base(serverFixture)
    {
    }

    [Fact]
    public async Task Server_ShouldStart_AndRespondToHealthCheck()
    {
        var response = await HttpClient.GetAsync("system/health");
        
        response.IsSuccessStatusCode.Should().BeTrue();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }
}
