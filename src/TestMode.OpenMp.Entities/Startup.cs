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
    public void OnGameModeInit(IWorldService world)
    {
        Console.WriteLine("whoop!");

        var vehicle = world.CreateVehicle(VehicleModelType.Landstalker, new Vector3(0, 2, 15), 0, 4, 4);
        vehicle.ChangeColor(5, 12);
        vehicle.Bonnet = true;
        vehicle.SetNumberPlate("SampSharp");
    }

    [Event]
    public void OnVehicleSpawn(Vehicle vehicle)
    {
          Console.WriteLine($"Vehicle {vehicle.Id} spawned!");
    }
    
    [Event]
    public void OnVehicleStreamIn(Vehicle vehicle, Player player)
    {
        Console.WriteLine($"Vehicle {vehicle.Id} streams in for player {player}");
    }
    
    [Event]
    public void OnVehicleStreamOut(Vehicle vehicle, Player player)
    {
        Console.WriteLine($"Vehicle {vehicle.Id} streams out for player {player}");
    }
}