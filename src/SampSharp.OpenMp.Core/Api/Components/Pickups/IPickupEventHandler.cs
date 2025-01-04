namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPickupEventHandler
{
    void OnPlayerPickUpPickup(IPlayer player, IPickup pickup);
}