namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IComponent))]
public readonly partial struct IDialogsComponent
{
    public static UID ComponentId => new(0x44a111350d611dde);

    public partial IEventDispatcher2<IPlayerDialogEventHandler> GetEventDispatcher();
}