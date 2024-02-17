namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerStreamEventHandler
{
    void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer);
    void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer);
}