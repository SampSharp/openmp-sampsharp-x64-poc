using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class OmpEntityProvider : IOmpEntityProvider
{
    private readonly IEntityManager _entityManager;
    private readonly IVehiclesComponent _vehicles;

    public OmpEntityProvider(OpenMp omp, IEntityManager entityManager)
    {
        _entityManager = entityManager;
        _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
    }

    public EntityId GetEntity(IVehicle vehicle)
    {
        return GetComponent(vehicle)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayer player)
    {
        return GetComponent(player)?.Entity ?? default;
    }

    public EntityId GetEntity(IPlayerObject playerObject)
    {
        // TODO implement
        return default;
    }

    public EntityId GetEntity(IObject @object)
    {
        // TODO implement
        return default;
    }

    public Player? GetComponent(IPlayer player)
    {
        if (player.Handle == 0)
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

    public Vehicle? GetComponent(IVehicle vehicle)
    {
        if (vehicle.Handle == 0)
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

    public Vehicle? GetVehicle(int id)
    {
        return GetComponent(_vehicles.AsPool().Get(id));
    }
}
