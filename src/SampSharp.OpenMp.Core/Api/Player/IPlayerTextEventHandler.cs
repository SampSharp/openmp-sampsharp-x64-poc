namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerTextEventHandler
{
    bool OnPlayerText(IPlayer player, StringView message);
    bool OnPlayerCommandText(IPlayer player, StringView message);
}