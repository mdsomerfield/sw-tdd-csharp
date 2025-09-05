using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Wordle.Api.Tests.Framework;

public class TestApplicationClient : WebApplicationFactory<Startup>
{
    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var testConfig = new Dictionary<string, string>();

        builder.ConfigureAppConfiguration(b =>
        {
            b.AddInMemoryCollection(testConfig.Select(t =>
                new KeyValuePair<string, string?>(t.Key, t.Value)));
        });
    }
}