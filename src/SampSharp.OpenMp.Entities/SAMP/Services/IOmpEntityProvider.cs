using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>
/// Provides methods for getting ECS entities/components for open.mp entities. For entities created through
/// SampSharp.Entities, the existing entities and components are returned. For foreign entities (entities created
/// through other scripts or open.mp components) new SampSharp.Entities entities and components are created and returned
/// where possible.
/// </summary>
public interface IOmpEntityProvider
{
    /// <summary>
    /// Gets the entity for the specified actor.
    /// </summary>
    /// <param name="actor">The actor to get the entity for.</param>
    /// <returns>The actor entity.</returns>
    EntityId GetEntity(IActor actor);
    
    /// <summary>
    /// Gets the entity for the specified gang zone.
    /// </summary>
    /// <param name="gangZone">The gang zone to get the entity for.</param>
    /// <returns>The gang zone entity.</returns>
    EntityId GetEntity(IGangZone gangZone);
    
    /// <summary>
    /// Gets the entity for the specified menu.
    /// </summary>
    /// <param name="menu">The menu to get the entity for.</param>
    /// <returns>The menu entity.</returns>
    EntityId GetEntity(IMenu menu);

    /// <summary>
    /// Gets the entity for the specified object.
    /// </summary>
    /// <param name="object">The object to get the entity for.</param>
    /// <returns>The object entity.</returns>
    EntityId GetEntity(IObject @object);
    
    /// <summary>
    /// Gets the entity for the specified pickup.
    /// </summary>
    /// <param name="pickup">The pickup to get the entity for.</param>
    /// <returns>The pickup entity.</returns>
    EntityId GetEntity(IPickup pickup);

    /// <summary>
    /// Gets the entity for the specified player.
    /// </summary>
    /// <param name="player">The player to get the entity for.</param>
    /// <returns>The player entity.</returns>
    EntityId GetEntity(IPlayer player);
    
    /// <summary>
    /// Gets the entity for the specified player object.
    /// </summary>
    /// <param name="playerObject">The player object to get the entity for.</param>
    /// <returns>The player object entity.</returns>
    EntityId GetEntity(IPlayerObject playerObject);

    /// <summary>
    /// Gets the entity for the specified vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to get the entity for.</param>
    /// <returns>The vehicle entity.</returns>
    EntityId GetEntity(IVehicle vehicle);
    
    /// <summary>
    /// Gets the component for the specified actor.
    /// </summary>
    /// <param name="actor">The actor to get the component for.</param>
    /// <returns>The actor component.</returns>
    Actor? GetComponent(IActor actor);

    /// <summary>
    /// Gets the component for the specified gang zone.
    /// </summary>
    /// <param name="gangZone">The gang zone to get the component for.</param>
    /// <returns>The gang zone component.</returns>
    GangZone? GetComponent(IGangZone gangZone);
    
    /// <summary>
    /// Gets the component for the specified menu.
    /// </summary>
    /// <param name="menu">The menu to get the component for.</param>
    /// <returns>The menu component.</returns>
    Menu? GetComponent(IMenu menu);

    /// <summary>
    /// Gets the component for the specified object.
    /// </summary>
    /// <param name="object">The object to get the component for.</param>
    /// <returns>The object component.</returns>
    GlobalObject? GetComponent(IObject @object);
    
    /// <summary>
    /// Gets the component for the specified pickup.
    /// </summary>
    /// <param name="pickup">The pickup to get the component for.</param>
    /// <returns>The pickup component.</returns>
    Pickup? GetComponent(IPickup pickup);

    /// <summary>
    /// Gets the component for the specified player.
    /// </summary>
    /// <param name="player">The player to get the component for.</param>
    /// <returns>The player component.</returns>
    Player? GetComponent(IPlayer player);
    
    /// <summary>
    /// Gets the component for the specified player object.
    /// </summary>
    /// <param name="playerObject">The player object to get the component for.</param>
    /// <returns>The player object component.</returns>
    PlayerObject? GetComponent(IPlayerObject playerObject);

    /// <summary>
    /// Gets the component for the specified vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to get the component for.</param>
    /// <returns>The vehicle component.</returns>
    Vehicle? GetComponent(IVehicle vehicle);

    //  TODO: all get by IDs
    Vehicle? GetVehicle(int id);
}