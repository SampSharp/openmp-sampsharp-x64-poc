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

        var vehicle = world.CreateVehicle(VehicleModelType.Landstalker, new Vector3(0, 2, 15), 0, 4, 4);
        vehicle.ChangeColor(5, 12);
        vehicle.Bonnet = true;
        vehicle.SetNumberPlate("SampSharp");

        vehicle.AddComponent<MyCustomComponent>();

        entityManager.AddComponent<MyCustomComponent>(EntityId.NewEntityId(), vehicle);
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
    public void OnConsoleCommandListRequest(ConsoleCommandCollection commands)
    {
        commands.Add("dump");
    }

    [Event]
    public bool OnConsoleText(string command, string args, ConsoleCommandSender sender, IEntityManager entityManager)
    {
        if (command == "dump")
        {
            foreach (var entity in entityManager.GetRootEntities())
            {
                DumpEntities(entityManager, entity, 0);
            }
            return true;
        }

        return false;

    }

    [Event]
    public void OnRconLoginAttempt(Player player, string password, bool success)
    {
        if (success)
        {
            player.AddComponent<AdminComponent>();
        }
    }

    private static void DumpEntities(IEntityManager entityManager, EntityId entity, int depth)
    {

        var ws = string.Concat(Enumerable.Repeat("| ", depth));

        if (depth > 0)
        {
            var ws2 = string.Concat(Enumerable.Repeat("| ", depth - 1));
            Console.WriteLine($"{ws2}+-E: {entity}");
        }
        else
        {
            Console.WriteLine($"E: {entity}");
        }

        foreach (var component in entityManager.GetComponents<Component>(entity))
        {
            Console.WriteLine($"{ws}+C: {component.GetType().Name} ({component})");
        }

        foreach (var child in entityManager.GetChildren(entity))
        {
            DumpEntities(entityManager, child, depth + 1);
        }
    }
}