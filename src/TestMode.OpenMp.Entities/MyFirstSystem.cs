using System.Numerics;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;

namespace TestMode.OpenMp.Entities;

public class MyFirstSystem : ISystem
{
    [Event]
    public void OnGameModeInit(IWorldService world, IEntityManager entityManager)
    {
        Console.WriteLine("whoop!");

        var vehicle = world.CreateVehicle(VehicleModelType.Landstalker, new Vector3(0, 6, 15), 0, 4, 4);
        vehicle.ChangeColor(5, 12);
        vehicle.Bonnet = true;
        vehicle.SetNumberPlate("SampSharp");

        vehicle.AddComponent<MyCustomComponent>();

        entityManager.AddComponent<MyCustomComponent>(EntityId.NewEntityId(), vehicle);
    }

    [Event]
    public bool OnPlayerCommandText(Player player, string cmdtext)
    {
        player.SendClientMessage(cmdtext);


        if (cmdtext.StartsWith("/help"))
        {
            return true;
        }

        return false;
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

    [Event]
    public void OnRconLoginAttempt(Player player, string password, bool success)
    {
        if (success)
        {
            player.AddComponent<AdminComponent>();
        }
    }
}