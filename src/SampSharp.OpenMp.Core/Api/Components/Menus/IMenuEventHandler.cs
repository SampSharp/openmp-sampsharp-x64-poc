namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IMenuEventHandler
{
    void OnPlayerSelectedMenuRow(IPlayer player, byte row);
    void OnPlayerExitedMenu(IPlayer player);
}