namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="ITextDrawsComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface ITextDrawEventHandler
{
    void OnPlayerClickTextDraw(IPlayer player, ITextDraw td);
    void OnPlayerClickPlayerTextDraw(IPlayer player, IPlayerTextDraw td);
    bool OnPlayerCancelTextDrawSelection(IPlayer player);
    bool OnPlayerCancelPlayerTextDrawSelection(IPlayer player);
}