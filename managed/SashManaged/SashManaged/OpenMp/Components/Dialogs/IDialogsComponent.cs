namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IDialogsComponent
{
    public static UID ComponentId => new(0x44a111350d611dde);

    public partial IEventDispatcher<IPlayerDialogEventHandler> GetEventDispatcher();
}