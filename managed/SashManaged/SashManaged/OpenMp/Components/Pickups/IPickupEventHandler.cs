namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPickupEventHandler
{
    void OnPlayerPickUpPickup(IPlayer player, IPickup pickup);
}