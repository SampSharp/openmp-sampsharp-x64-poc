namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides extension methods for <see cref="IEventDispatcher{T}" />.
/// </summary>
public static class EventDispatcherExtensions
{
    /// <summary>
    /// Adds the specified <paramref name="handler" /> to the event dispatcher and returns a disposable object that can be used to remove the handler.
    /// </summary>
    /// <typeparam name="T">The type of the event handler.</typeparam>
    /// <param name="dispatcher">The event dispatcher to add the handler to.</param>
    /// <param name="handler">The handler to add to the dispatcher.</param>
    /// <param name="priority">The priority at which the handler should receive the events.</param>
    /// <returns>An object which when disposed, removes the handler from the dispatcher.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified handler was already added to the event dispatcher.</exception>
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