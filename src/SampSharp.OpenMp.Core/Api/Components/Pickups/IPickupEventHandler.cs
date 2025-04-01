namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPickupsComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IPickupEventHandler
{
    void OnPlayerPickUpPickup(IPlayer player, IPickup pickup);
}