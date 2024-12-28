namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IMenuEventHandler : IEventHandler2
{
    void OnPlayerSelectedMenuRow(IPlayer player, byte row);
    void OnPlayerExitedMenu(IPlayer player);
}