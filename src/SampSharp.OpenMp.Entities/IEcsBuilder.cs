namespace SampSharp.Entities;

/// <summary>Provides functionality for configuring the SampSharp EntityComponentSystem.</summary>
public interface IEcsBuilder
{
    /// <summary>Gets the service provider.</summary>
    IServiceProvider Services { get; }

    /// <summary>Adds a middleware to the handler of the event with the specified <paramref name="name" />.</summary>
    /// <param name="name">The name of the event.</param>
    /// <param name="middleware">The middleware to add to the event.</param>
    /// <returns>The builder.</returns>
    IEcsBuilder UseMiddleware(string name, Func<EventDelegate, EventDelegate> middleware);
}