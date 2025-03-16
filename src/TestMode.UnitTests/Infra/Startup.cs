using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;

namespace TestMode.UnitTests;

public class Startup : IEcsStartup
{
    public void Initialize(IStartupContext context)
    {
        context.UseEntities();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<TestManager>();
        services.AddSystemsInAssembly();
    }

    public void Configure(IEcsBuilder builder)
    {
    }
}