using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class EntityProvider : IEntityProvider
{
    private readonly IEntityManager _entityManager;
    private readonly IVehiclesComponent _vehicles;

    public EntityProvider(OpenMp omp, IEntityManager entityManager)
    {
        _entityManager = entityManager;
        _vehicles = omp.Components.QueryComponent<IVehiclesComponent>();
    }

    public EntityId GetEntity(IVehicle vehicle)
    {
        var ext = vehicle.TryGetExtension<ComponentExtension>();

        if (ext == null)
        {
            var component = _entityManager.AddComponent<Vehicle>(EntityId.NewEntityId(), _vehicles, vehicle);
            ext = new ComponentExtension(component);
            vehicle.AddExtension(ext);

            return component;
        }

        return ext.Component.Entity;
    }

    public EntityId GetEntity(IPlayer player)
    {
        var ext = player.TryGetExtension<ComponentExtension>();
        if (ext == null)
        {
            var component = _entityManager.AddComponent<Player>(EntityId.NewEntityId(), player);
            ext = new ComponentExtension(component);
            player.AddExtension(ext);
            return component;
        }
        return ext.Component.Entity;
    }
}
