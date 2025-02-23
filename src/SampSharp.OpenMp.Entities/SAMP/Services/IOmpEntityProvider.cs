using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>
/// Provides methods for getting ECS entities for open.mp entities.
/// </summary>
public interface IOmpEntityProvider
{
    /// <summary>
    /// Gets the entity for the specified vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to get the entity for.</param>
    /// <returns>The vehicle entity.</returns>
    EntityId GetEntity(IVehicle vehicle);

    /// <summary>
    /// Gets the entity for the specified player.
    /// </summary>
    /// <param name="player">The player to get the entity for.</param>
    /// <returns>The player entity.</returns>
    EntityId GetEntity(IPlayer player);

    /// <summary>
    /// Gets the component for the specified player.
    /// </summary>
    /// <param name="player">The player to get the component for.</param>
    /// <returns>The player component.</returns>
    Player GetComponent(IPlayer player);
}