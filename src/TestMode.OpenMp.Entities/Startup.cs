using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;

namespace TestMode.OpenMp.Entities;

public class Startup : IEcsStartup
{
    public void Initialize(StartupContext context)
    {
        context.UseEntities(cfg =>
        {
            cfg.ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Information));
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSystemsInAssembly();
    }

    public void Configure(IEcsBuilder builder)
    {
    }
}