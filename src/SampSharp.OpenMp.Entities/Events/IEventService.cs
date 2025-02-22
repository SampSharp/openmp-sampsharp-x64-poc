namespace SampSharp.Entities;

/// <summary>Provides functionality for handling events.</summary>
public interface IEventService
{
    /// <summary>Enables handling of the callback with the specified <paramref name="name" /> as an event.</summary>
    /// <param name="name">The name of the callback.</param>
    /// <param name="parameters">The types of the parameters of the callback.</param>
    void EnableEvent(string name, params Type[] parameters);

    /// <summary>Adds a middleware to the handler of the event with the specified <paramref name="name" />.</summary>
    /// <param name="name">The name of the event.</param>
    /// <param name="middleware">The middleware to add to the event.</param>
    void UseMiddleware(string name, Func<EventDelegate, EventDelegate> middleware);

    /// <summary>Invokes the event with the specified <paramref name="name" /> and <paramref name="arguments" />.</summary>
    /// <param name="name">The name of the event.</param>
    /// <param name="arguments">The arguments of the event.</param>
    /// <returns>The result of the event.</returns>
    object? Invoke(string name, params object[] arguments);
}