namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface ITextDrawEventHandler
{
    void OnPlayerClickTextDraw(IPlayer player, ITextDraw td) { }
    void OnPlayerClickPlayerTextDraw(IPlayer player, IPlayerTextDraw td) { }
    bool OnPlayerCancelTextDrawSelection(IPlayer player) { return false; }
    bool OnPlayerCancelPlayerTextDrawSelection(IPlayer player) { return false; }
};