namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerStreamEventHandler : IEventHandler2
{
    void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer);
    void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer);
}