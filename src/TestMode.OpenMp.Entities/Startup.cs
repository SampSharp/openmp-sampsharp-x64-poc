using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;

namespace TestMode.OpenMp.Entities;

public class Startup : IEcsStartup
{
    public void Initialize(StartupContext context)
    {
        context.ForwardConsoleOutputToOpenMpLogger();
        context.UseEntities();
    }

    public void Configure(IServiceCollection services)
    {
        services
            .AddSystem<TestTicker>()
            .AddSystem<MyFirstSystem>();
    }

    public void Configure(IEcsBuilder builder)
    {
    }
}