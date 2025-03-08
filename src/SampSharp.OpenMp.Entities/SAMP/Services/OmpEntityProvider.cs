using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class OmpEntityProvider : IOmpEntityProvider
{
    private readonly IEntityManager _entityManager;
    private readonly IVehiclesComponent _vehicles;
    private readonly IObjectsComponent _objects;
    private readonly IGangZonesComponent _gangZones;
    private readonly IActorsComponent _actors;
    private readonly IPickupsComponent _pickups;
    private readonly ITextDrawsComponent _textDraws;
    private readonly IMenusComponent _menus;
    private readonly IPlayerPool _players;
    public OmpEntityProvider(OpenMp omp, IEntityManager entityManager)
    {
        _entityManager = entityManager;
        _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
        _objects = omp.Components.QueryComponent<IObjectsComponent>();
        _gangZones = omp.Components.QueryComponent<IGangZonesComponent>();
        _actors = omp.Components.QueryComponent<IActorsComponent>();
        _pickups = omp.Components.QueryComponent<IPickupsComponent>();
        _textDraws = omp.Components.QueryComponent<ITextDrawsComponent>();
        _menus = omp.Components.QueryComponent<IMenusComponent>();
        _players = omp.Core.GetPlayers();
    }

    public EntityId GetEntity(IActor actor)
    {
        return GetComponent(actor)?.Entity ?? default;
    }

    public EntityId GetEntity(IGangZone gangZone)
    {
        return GetComponent(gangZone)?.Entity ?? default;
    }

    public EntityId GetEntity(IMenu menu)
    {
        return GetComponent(menu)?.Entity ?? default;
    }

    public EntityId GetEntity(IObject @object)
    {
        return GetComponent(@object)?.Entity ?? default;
    }

    public EntityId GetEntity(IPickup pickup)
    {
        return GetComponent(pickup)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayer player)
    {
        return GetComponent(player)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerObject playerObject)
    {
        return GetComponent(playerObject)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerTextDraw playerTextDraw)
    {
        return GetComponent(playerTextDraw)?.Entity ?? default;
    }

    public EntityId GetEntity(ITextDraw textDraw)
    {
        return GetComponent(textDraw)?.Entity ?? default;
    }

    public EntityId GetEntity(IVehicle vehicle)
    {
        return GetComponent(vehicle)?.Entity ?? default;
    }

    public Actor? GetComponent(IActor actor)
    {
        if (actor == null)
        {
            return null;
        }
        var ext = actor.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = _entityManager.AddComponent<Actor>(EntityId.NewEntityId(), _actors, actor);
            ext = new ComponentExtension(component);
            actor.AddExtension(ext);

            return component;
        }
        return (Actor)ext.Component;
    }

    public GangZone? GetComponent(IGangZone gangZone)
    {
        if (gangZone == null)
        {
            return null;
        }
        var ext = gangZone.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = _entityManager.AddComponent<GangZone>(EntityId.NewEntityId(), _gangZones, gangZone);
            ext = new ComponentExtension(component);
            gangZone.AddExtension(ext);

            return component;
        }
        return (GangZone)ext.Component;
    }

    public Menu? GetComponent(IMenu menu)
    {
        if (menu == null)
        {
            return null;
        }
        var ext = menu.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            // don't know the title of the menu (which cannot be retrieved through open.mp api) - cannot create a component for the foreign entity.
            return null;
        }
        return (Menu)ext.Component;
    }

    public GlobalObject? GetComponent(IObject @object)
    {
        if (@object == null)
        {
            return null;
        }
        var ext = @object.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {

            var component = _entityManager.AddComponent<GlobalObject>(EntityId.NewEntityId(), _objects, @object);
            ext = new ComponentExtension(component);
            @object.AddExtension(ext);

            return component;
        }
        return (GlobalObject)ext.Component;
    }

    public Pickup? GetComponent(IPickup pickup)
    {
        if (pickup == null)
        {
            return null;
        }
        var ext = pickup.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = _entityManager.AddComponent<Pickup>(EntityId.NewEntityId(), _pickups, pickup);
            ext = new ComponentExtension(component);
            pickup.AddExtension(ext);

            return component;
        }
        return (Pickup)ext.Component;
    }

    public Player? GetComponent(IPlayer player)
    {
        if (player == null)
        {
            return null;
        }
        var ext = player.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = _entityManager.AddComponent<Player>(EntityId.NewEntityId(), this, player);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }
        return (Player)ext.Component;
    }

    public PlayerObject? GetComponent(IPlayerObject playerObject)
    {
        if (playerObject == null)
        {
            return null;
        }
        var ext = playerObject.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            // don't know for which player this object is created - cannot create a component for the foreign entity.
            return null;
        }
        return (PlayerObject)ext.Component;
    }

    public PlayerTextDraw? GetComponent(IPlayerTextDraw playerTextDraw)
    {
        if (playerTextDraw == null)
        {
            return null;
        }
        var ext = playerTextDraw.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            // don't know for which player this text draw is created - cannot create a component for the foreign entity.
            return null;
        }

        return (PlayerTextDraw)ext.Component;
    }

    public TextDraw? GetComponent(ITextDraw textDraw)
    {
        if (textDraw == null)
        {
            return null;
        }
        var ext = textDraw.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = _entityManager.AddComponent<TextDraw>(EntityId.NewEntityId(), _textDraws, textDraw);
            ext = new ComponentExtension(component);
            textDraw.AddExtension(ext);

            return component;
        }

        return (TextDraw)ext.Component;
    }

    public Vehicle? GetComponent(IVehicle vehicle)
    {
        if (vehicle == null)
        {
            return null;
        }

        var ext = vehicle.TryGetExtension<ComponentExtension>();

        if (ext == null)
        {
            var component = _entityManager.AddComponent<Vehicle>(EntityId.NewEntityId(), _vehicles, vehicle);
            ext = new ComponentExtension(component);
            vehicle.AddExtension(ext);

            return component;
        }

        return (Vehicle)ext.Component;
    }

    public Player? GetPlayer(int id)
    {
        return GetComponent(_players.Get(id));
    }

    public Vehicle? GetVehicle(int id)
    {
        return GetComponent(_vehicles.AsPool().Get(id));
    }

    public Menu? GetMenu(int id)
    {
        return GetComponent(_menus.AsPool().Get(id));
    }
}
