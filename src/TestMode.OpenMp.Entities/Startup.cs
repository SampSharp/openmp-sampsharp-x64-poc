using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
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

public class MyFirstSystem : ISystem
{
    [Event]
    public void OnInitialized(IWorldService world)
    {
        var vehicle = world.CreateVehicle(VehicleModelType.Landstalker, Vector3.UnitZ * 15 + Vector3.UnitX, 0, 4, 4);
        vehicle.ChangeColor(5, 12);
        vehicle.Bonnet = true;
        vehicle.SetNumberPlate("SampSharp");
    }
}