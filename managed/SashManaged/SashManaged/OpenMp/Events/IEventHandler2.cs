namespace SashManaged.OpenMp;

public interface IEventHandler2
{
    nint IncreaseReference();
    void DecreaseReference();

    nint? GetHandle()
    {
        return EventHandlerNativeHandleStorage.GetHandle(this);
    }
}