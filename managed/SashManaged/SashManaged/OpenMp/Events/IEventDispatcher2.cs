using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IEventDispatcher2<T> : IPointer where T : IEventHandler2
{
    private readonly nint _handle;

    public nint Handle => _handle;

    public bool AddEventHandler(T handler, EventPriority priority = EventPriority.Default)
    {
        var handlerHandle = handler.IncreaseReference();

        return EventDispatcherInterop.AddEventHandler(_handle, handlerHandle, priority);
    }

    public bool RemoveEventHandler(T handler)
    {
        var handlerHandle = handler.GetHandle() ?? throw new InvalidOperationException("Missing handle.");

        if (EventDispatcherInterop.RemoveEventHandler(_handle, handlerHandle))
        {
            handler.DecreaseReference();
            return true;
        }

        return false;
    }

    public bool HasEventHandler(T handler, out EventPriority priority)
    {
        var handlerHandle = handler.GetHandle();
        priority = default;

        return handlerHandle.HasValue && EventDispatcherInterop.HasEventHandler(_handle, handlerHandle.Value, out priority);
    }

    public Size Count()
    {
        return EventDispatcherInterop.Count(_handle);
    }
}