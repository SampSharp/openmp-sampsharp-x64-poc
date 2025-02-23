using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

public class WorldService : IWorldService
{
    private readonly ICore _core;
    private readonly IPlayerPool _players;
    private readonly IVehiclesComponent _vehicles;
    private readonly IObjectsComponent _objects;
    private readonly IEntityManager _entityManager;

    public WorldService(OpenMp omp, IEntityManager entityManager)
    {
        _core = omp.Core;
        _players = omp.Core.GetPlayers();
        _entityManager = entityManager;
        _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
        _objects = omp.Components.QueryComponent<IObjectsComponent>();
    }

    public float Gravity
    {
        get => _core.GetGravity();
        set => _core.SetGravity(value);
    }

    public Vehicle CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false, EntityId parent = default)
    {
        return CreateVehicle(false, type, position, rotation, color1, color2, respawnDelay, addSiren, parent);
    }

    public Vehicle CreateStaticVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false,
        EntityId parent = default)
    {
        return CreateVehicle(true, type, position, rotation, color1, color2, respawnDelay, addSiren, parent);
    }

    private Vehicle CreateVehicle(bool isStatic, VehicleModelType type, Vector3 position, float rotation, int color1, int color2, int respawnDelay = -1, bool addSiren = false,
        EntityId parent = default)
    {
        var native = _vehicles.Create(isStatic, (int)type, position, rotation, color1, color2, respawnDelay, addSiren);

        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<Vehicle>(entityId, parent, _vehicles, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
    }

    public void SetObjectsDefaultCameraCollision(bool disable)
    {
        _objects.SetDefaultCameraCollision(disable);
    }

    public void SendClientMessage(Colour color, string message)
    {
        _players.SendClientMessageToAll(ref color, message);
    }

    public void SendClientMessage(Colour color, string messageFormat, params object[] args)
    {
        var message = string.Format(messageFormat, args);
        SendClientMessage(color, message);
    }

    public void SendClientMessage(string message)
    {
        SendClientMessage(new Colour(0xff, 0xff, 0xff, 0xff), message);
    }

    public void SendClientMessage(string messageFormat, params object[] args)
    {
        var message = string.Format(messageFormat, args);
        SendClientMessage(message);
    }

    public void SendPlayerMessageToPlayer(Player sender, string message)
    {
        _players.SendChatMessageToAll(sender.Native, message);
    }

    public void SendDeathMessage(Player killer, Player killee, Weapon weapon)
    {
        _players.SendDeathMessageToAll(killer.Native, killee.Native, (int)weapon);
    }

    [Obsolete("Use GameText(string, TimeSpan, int) instead.")]
    public void GameText(string text, int time, int style)
    {
        GameText(text, TimeSpan.FromMilliseconds(time), style);
    }
    public void GameText(string text, TimeSpan time, int style)
    {
        // TODO: style enum?
        _players.SendGameTextToAll(text, time, style);
    }

    public void CreateExplosion(Vector3 position, ExplosionType type, float radius)
    {
        _players.CreateExplosionForAll(position, (int)type, radius);
    }

    public void SetWeather(int weather)
    {
        _core.SetWeather(weather);
    }
}