namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IMenusComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IMenuEventHandler
{
    void OnPlayerSelectedMenuRow(IPlayer player, byte row);
    void OnPlayerExitedMenu(IPlayer player);
}