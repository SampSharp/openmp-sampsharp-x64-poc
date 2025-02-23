using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>
/// Provides methods for getting ECS entities for open.mp entities.
/// </summary>
public interface IEntityProvider
{
    /// <summary>
    /// Gets the component for the specified vehicle.
    /// </summary>
    /// <param name="vehicle">The vehicle to get the component for.</param>
    /// <returns>The vehicle component.</returns>
    EntityId GetEntity(IVehicle vehicle);

    /// <summary>
    /// Gets the component for the specified player.
    /// </summary>
    /// <param name="player">The player to get the component for.</param>
    /// <returns>The player component.</returns>
    EntityId GetEntity(IPlayer player);
}