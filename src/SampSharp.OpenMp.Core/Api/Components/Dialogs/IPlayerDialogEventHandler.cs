namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IDialogsComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerDialogEventHandler
{
    void OnDialogResponse(IPlayer player, int dialogId, DialogResponse response, int listItem, string inputText);
}