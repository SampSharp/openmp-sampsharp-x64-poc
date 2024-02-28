namespace SashManaged.OpenMp;

internal static class EventHandlerNativeHandleStorage
{
    private static readonly Dictionary<IEventHandler2, (nint handle, int references, Delegate[] delegates)> _refs = [];

    public static nint? GetAndIncreaseReference(IEventHandler2 handler)
    {
        if (_refs.TryGetValue(handler, out var tuple))
        {
            _refs[handler] = (tuple.handle, tuple.references + 1, tuple.delegates);
            return tuple.handle;
        }

        return null;
    }

    public static void CreateReference(IEventHandler2 handler, nint handle, params Delegate[] delegates)
    {
        _refs[handler] = (handle, 1, delegates);
    }

    public static nint? GetHandle(IEventHandler2 handler)
    {
        return _refs.TryGetValue(handler, out var tuple) ? tuple.handle : null;
    }

    /// <summary>
    /// Returns handle when handle should be destroyed.
    /// </summary>
    public static nint? GetAndDecreaseReference(IEventHandler2 handler)
    {
        if (!_refs.TryGetValue(handler, out var tuple))
        {
            return null;
        }

        if (tuple.references == 1)
        {
            _refs.Remove(handler);
            return tuple.handle;
        }

        _refs[handler] = (tuple.handle, tuple.references - 1, tuple.delegates);
        return null;
    }
}