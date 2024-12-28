namespace SashManaged.OpenMp;

[OpenMpEventHandler2]
public interface IPlayerDialogEventHandler : IEventHandler2
{
    void OnDialogResponse(IPlayer player, int dialogId, DialogResponse response, int listItem, StringView inputText);
}