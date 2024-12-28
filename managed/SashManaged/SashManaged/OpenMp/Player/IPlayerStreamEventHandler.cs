namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public partial interface IPlayerStreamEventHandler
{
    void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer);
    void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer);
}