namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IMenuEventHandler
{
    void OnPlayerSelectedMenuRow(IPlayer player, byte row);
    void OnPlayerExitedMenu(IPlayer player);
}