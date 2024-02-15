namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPickupEventHandler
{
    void OnPlayerPickUpPickup(IPlayer player, IPickup pickup);
}