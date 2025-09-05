namespace Wordle.Api.Domains.System;

public struct HealthCheckResponse
{
    public bool Ok { get; set; }
    public DateTime Timestamp { get; set; }
}