namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerTextEventHandler
{
    bool OnPlayerText(IPlayer player, string message);
    bool OnPlayerCommandText(IPlayer player, string message);
}