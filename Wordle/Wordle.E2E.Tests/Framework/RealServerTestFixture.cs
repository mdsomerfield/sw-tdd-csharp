using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Wordle.E2E.Tests.Framework;

public class RealServerTestFixture : IAsyncLifetime
{
    private IHost? _webHost;
    private readonly int _webPort;
    public string WebBaseUrl => $"http://localhost:{_webPort}";

    public RealServerTestFixture()
    {
        _webPort = GetAvailablePort();
    }

    public async Task InitializeAsync()
    {
        var webArgs = new[] { $"--urls={WebBaseUrl}" };
        _webHost = Web.Program.CreateHostBuilder(webArgs)
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ApiBaseUrl"] = WebBaseUrl
                });
            })
            .Build();
        await _webHost.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_webHost != null)
        {
            await _webHost.StopAsync();
            _webHost.Dispose();
        }
    }

    private static int GetAvailablePort()
    {
        using var socket = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
        socket.Start();
        var port = ((System.Net.IPEndPoint)socket.LocalEndpoint).Port;
        socket.Stop();
        return port;
    }
}