namespace SampSharp.OpenMp.Core.Api;

public static class EventDispatcherExtensions
{
    public static IDisposable Add<T>(this IEventDispatcher<T> dispatcher, T handler, EventPriority priority = EventPriority.Default)
        where T : class, IEventHandler<T>
    {
        if (!dispatcher.AddEventHandler(handler, priority))
        {
            throw new InvalidOperationException("Failed to add event handler.");
        }

        return new DisposableHandler<T>(dispatcher, handler);
    }

    private class DisposableHandler<T>(IEventDispatcher<T> dispatcher, T handler) : IDisposable
        where T : class, IEventHandler<T>
    {
        private bool _disposed;
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            dispatcher.RemoveEventHandler(handler);
        }
    }
}