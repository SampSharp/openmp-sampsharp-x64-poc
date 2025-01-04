namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerStreamEventHandler
{
    void OnPlayerStreamIn(IPlayer player, IPlayer forPlayer);
    void OnPlayerStreamOut(IPlayer player, IPlayer forPlayer);
}