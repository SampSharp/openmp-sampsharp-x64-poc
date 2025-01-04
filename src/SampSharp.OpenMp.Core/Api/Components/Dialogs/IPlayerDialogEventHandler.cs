namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerDialogEventHandler
{
    void OnDialogResponse(IPlayer player, int dialogId, DialogResponse response, int listItem, StringView inputText);
}