using FluentAssertions;
using Wordle.Api.Domains.System;
using Wordle.Api.Tests.Framework;
using Xunit;

namespace Wordle.Api.Tests.Tests;

[Collection("ApiTest")]
public class HealthCheckTests
{
    private readonly ApiTestFixture _fixture;

    public HealthCheckTests(ApiTestFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task HelloWorld()
    {
        var client = _fixture.CreateJsonClient();
        var response = await client.GetAsync<HealthCheckResponse>("/system/health");
        response.Ok.Should().Be(true);
    }

}
