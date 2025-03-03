using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

public class WorldService : IWorldService
{
    private readonly ICore _core;
    private readonly IPlayerPool _players;
    private readonly IVehiclesComponent _vehicles;
    private readonly IObjectsComponent _objects;
    private readonly IActorsComponent _actors;
    private readonly IGangZonesComponent _gangZones;
    private readonly IMenusComponent _menus;
    private readonly IPickupsComponent _pickups;
    private readonly IEntityManager _entityManager;

    public WorldService(OpenMp omp, IEntityManager entityManager)
    {
        _core = omp.Core;
        _players = omp.Core.GetPlayers();
        _entityManager = entityManager;
        _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
        _objects = omp.Components.QueryComponent<IObjectsComponent>();
        _actors = omp.Components.QueryComponent<IActorsComponent>();
        _gangZones = omp.Components.QueryComponent<IGangZonesComponent>();
        _menus = omp.Components.QueryComponent<IMenusComponent>();
        _pickups = omp.Components.QueryComponent<IPickupsComponent>();
    }

    public float Gravity
    {
        get => _core.GetGravity();
        set => _core.SetGravity(value);
    }

    public Actor CreateActor(int modelId, Vector3 position, float rotation, EntityId parent = default)
    {
        var native = _actors.Create(modelId, position, rotation);

        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<Actor>(entityId, parent, _actors, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
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

    public GangZone CreateGangZone(float minX, float minY, float maxX, float maxY, EntityId parent = default)
    {
        return CreateGangZone(new Vector2(minX, minY), new Vector2(maxX, maxY), parent);
    }
    
    public GangZone CreateGangZone(Vector2 min, Vector2 max, EntityId parent = default)
    {
        var native = _gangZones.Create(new GangZonePos(min, max));
        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<GangZone>(entityId, parent, _gangZones, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
    }

    public Pickup CreatePickup(int model, PickupType type, Vector3 position, int virtualWorld = -1, EntityId parent = default)
    {
        var native = _pickups.Create(model, (byte)type, position, (uint)virtualWorld, false);
        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<Pickup>(entityId, parent, _objects, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
    }

    public Pickup CreateStaticPickup(int model, PickupType type, Vector3 position, int virtualWorld = -1, EntityId parent = default)
    {
        var native = _pickups.Create(model, (byte)type, position, (uint)virtualWorld, true);
        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<Pickup>(entityId, parent, _objects, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
    }

    public GlobalObject CreateObject(int modelId, Vector3 position, Vector3 rotation, float drawDistance = 0, EntityId parent = default)
    {
        var native = _objects.Create(modelId, position, rotation, drawDistance);
        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<GlobalObject>(entityId, parent, _objects, native);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
    }

    public Menu CreateMenu(string title, Vector2 position, float col0Width, float? col1Width = null, EntityId parent = default)
    {
        var native = _menus.Create(title, position, col1Width.HasValue ? (byte)2 : (byte)1, col0Width, col1Width ?? 0);
        var entityId = EntityId.NewEntityId();
        var component = _entityManager.AddComponent<Menu>(entityId, parent, _menus, native, title);

        var extension = new ComponentExtension(component);
        native.AddExtension(extension);

        return component;
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
        _players.SendChatMessageToAll(sender, message);
    }

    public void SendDeathMessage(Player killer, Player killee, Weapon weapon)
    {
        _players.SendDeathMessageToAll(killer, killee, (int)weapon);
    }

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