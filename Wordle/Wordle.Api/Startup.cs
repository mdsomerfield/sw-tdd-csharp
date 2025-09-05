using System.Text.Json.Serialization;

namespace Wordle.Api;

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        ;
        services.AddEndpointsApiExplorer();

        services.AddHttpContextAccessor();

        services.AddLogging(builder => builder.AddConsole());
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UsePathBase("/api");
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}