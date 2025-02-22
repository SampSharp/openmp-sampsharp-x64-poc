using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;

namespace TestMode.OpenMp.Entities;

public class Startup : IStartup, IEcsStartup
{
    public void Initialize(StartupContext context)
    {
        context.ForwardConsoleOutputToOpenMpLogger();
        context.UseEntities();
    }

    public void Configure(IServiceCollection services)
    {
        services.AddSystem<TestTicker>();
    }

    public void Configure(IEcsBuilder builder)
    {
    }
}