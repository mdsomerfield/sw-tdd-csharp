using Microsoft.AspNetCore.Mvc;

namespace Wordle.Api.Domains.System;

[ApiController]
[Route("system")]
public class SystemController : ControllerBase
{
    [HttpGet("health")]
    public HealthCheckResponse HealthCheck()
    {
        throw new InvalidOperationException();
        var response = new HealthCheckResponse
        {
            Ok = true,
            Timestamp = DateTime.UtcNow
        };

        return response;
    }
}