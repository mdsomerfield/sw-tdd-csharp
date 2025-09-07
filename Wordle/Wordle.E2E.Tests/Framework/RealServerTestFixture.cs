using Microsoft.Extensions.Hosting;
using Wordle.Api;
using Xunit;

namespace Wordle.E2E.Tests.Framework;

public class RealServerTestFixture : IAsyncLifetime
{
    private IHost? _host;
    private readonly int _port;
    public string BaseUrl => $"http://localhost:{_port}";

    public RealServerTestFixture()
    {
        _port = GetAvailablePort();
    }

    public async Task InitializeAsync()
    {
        var args = new[] { $"--urls={BaseUrl}" };
        _host = Program.CreateHostBuilder(args).Build();
        await _host.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
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