namespace SashManaged.OpenMp;

public interface IEventHandler
{
    /// <summary>
    /// Do not call manually. Increases the reference counter to the unmanaged handle of this event handler and creates a handle if it does not exist yet.
    /// </summary>
    /// <returns>The unmanaged handle of this handler.</returns>
    nint IncreaseReference();

    /// <summary>
    /// Do not call manually. Reduces the reference counter to the unmanaged handle of this event handler and destroys the handle if the counter reaches zero.
    /// </summary>
    void DecreaseReference();

    /// <summary>
    /// Do not call manually. Gets the unmanaged handle of this handler.
    /// </summary>
    /// <returns>The unmanaged handle of this handler.</returns>
    nint? GetHandle()
    {
        return EventHandlerNativeHandleStorage.GetHandle(this);
    }
}