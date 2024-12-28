namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPickupEventHandler : IEventHandler2
{
    void OnPlayerPickUpPickup(IPlayer player, IPickup pickup);
}