namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface ITextDrawEventHandler : IEventHandler2
{
    void OnPlayerClickTextDraw(IPlayer player, ITextDraw td);
    void OnPlayerClickPlayerTextDraw(IPlayer player, IPlayerTextDraw td);
    bool OnPlayerCancelTextDrawSelection(IPlayer player);
    bool OnPlayerCancelPlayerTextDrawSelection(IPlayer player);
}