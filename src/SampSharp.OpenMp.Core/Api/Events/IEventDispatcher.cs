using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IEventDispatcher{T}"/> interface.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct IEventDispatcher<T> where T : class, IEventHandler<T>
{
    private readonly nint _handle;

    public IEventDispatcher(nint handle)
    {
        _handle = handle;
    }

    public bool AddEventHandler(T handler, EventPriority priority = EventPriority.Default)
    {
        var handlerHandle = T.Manager.Get(handler).Create();

        return EventDispatcherInterop.AddEventHandler(_handle, handlerHandle, priority);
    }

    public bool RemoveEventHandler(T handler)
    {
        var reference = T.Manager.Get(handler);
        var handlerHandle = reference.Handle;

        if (!handlerHandle.HasValue)
        {
            return false;
        }

        if (EventDispatcherInterop.RemoveEventHandler(_handle, handlerHandle.Value))
        {
            reference.Free();
            return true;
        }

        return false;
    }

    public bool HasEventHandler(T handler, out EventPriority priority)
    {
        var handlerHandle = T.Manager.Get(handler).Handle;
        priority = default;

        return handlerHandle.HasValue && EventDispatcherInterop.HasEventHandler(_handle, handlerHandle.Value, out priority);
    }

    public int Count()
    {
        return (int)EventDispatcherInterop.Count(_handle);
    }
}