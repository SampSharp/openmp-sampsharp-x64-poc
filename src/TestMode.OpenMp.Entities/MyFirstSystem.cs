using System.Numerics;
using Microsoft.Extensions.Logging;
using SampSharp.Entities;
using SampSharp.Entities.SAMP;
using SampSharp.OpenMp.Core.Api;
using PlayerRecordingType = SampSharp.Entities.SAMP.PlayerRecordingType;

namespace TestMode.OpenMp.Entities;

public class MyFirstSystem : ISystem
{
    private int _ticks;
    [Timer(1000)]
    public void OnTimer()
    {
        if (_ticks++ == 3)
        {
            //throw new Exception("Test exception");
        }
    }

    [Event]
    public void OnGameModeInit(IWorldService world, IEntityManager entityManager, ILogger<MyFirstSystem> logger)
    {
        logger.LogInformation("whoop!");

        var vehicle = world.CreateVehicle(VehicleModelType.Landstalker, new Vector3(0, 6, 15), 45, 4, 4);
        vehicle.ChangeColor(5, 12);
        vehicle.Bonnet = true;
        vehicle.SetNumberPlate("SampSharp");

        vehicle.AddComponent<MyCustomComponent>();

        entityManager.AddComponent<MyCustomComponent>(EntityId.NewEntityId(), vehicle);

        var actor = world.CreateActor(1, new Vector3(15, 0, 5), 0);

        var spawn = ((IActor)actor).GetSpawnData();
        Console.WriteLine(spawn);
    }

    [Event]
    public bool OnPlayerCommandText(Player player, string cmdtext, IDialogService dialogService)
    {
        player.SendClientMessage(cmdtext);


        if (cmdtext.StartsWith("/help"))
        {
            return true;
        }

        if (cmdtext == "/dialog-input")
        {
            var diag = new InputDialog("Input", "Enter your name", "OK", "Cancel");

            dialogService.Show(player, diag, r => player.SendClientMessage($"response: {r.Response}, {r.InputText ?? "<<NULL>>"}"));
            return true;
        }

        if (cmdtext == "/dialog-message")
        {
            var diag = new MessageDialog("Message", "This is a message dialog", "OK");

            dialogService.Show(player, diag, r => player.SendClientMessage($"response: {r.Response}"));
            return true;
        }

        if (cmdtext == "/dialog-list")
        {
            var diag = new ListDialog("List", "OK")
            {
                "A", "B", "C"
            };

            dialogService.Show(player, diag, r => player.SendClientMessage($"response: {r.Response} {r.ItemIndex} {r.Item?.Text ?? "<<NULL>>"}"));
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