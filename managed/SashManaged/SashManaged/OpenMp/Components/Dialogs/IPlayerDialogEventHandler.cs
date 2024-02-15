namespace SashManaged.OpenMp;

public interface IPlayerDialogEventHandler
{
    void OnDialogResponse(IPlayer player, int dialogId, DialogResponse response, int listItem, StringView inputText);
}