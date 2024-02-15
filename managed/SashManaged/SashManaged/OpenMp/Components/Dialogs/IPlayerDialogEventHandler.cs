namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerDialogEventHandler
{
    void OnDialogResponse(IPlayer player, int dialogId, DialogResponse response, int listItem, StringView inputText);
}