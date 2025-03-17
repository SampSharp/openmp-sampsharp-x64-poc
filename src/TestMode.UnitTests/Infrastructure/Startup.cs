using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.OpenMp.Core;
using Shouldly;

namespace TestMode.UnitTests;

public class Startup : IEcsStartup
{
    public void Initialize(IStartupContext context)
    {
        ShouldlyConfiguration.DefaultFloatingPointTolerance = 0.0005f;

        context.UseEntities();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSystemsInAssembly();
    }

    public void Configure(IEcsBuilder builder)
    {
    }
}