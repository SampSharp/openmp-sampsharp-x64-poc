namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Base interface for event handlers. This interface is automatically implemented by the code generator for event
/// handlers which are marked with the <see cref="OpenMpEventHandlerAttribute" />.
/// </summary>
/// <typeparam name="TEventHandler"></typeparam>
public interface IEventHandler<TEventHandler> where TEventHandler : class
{
    /// <summary>
    /// Gets the manager for the creation of native event handler handles.
    /// </summary>
    static abstract INativeEventHandlerManager<TEventHandler> Manager { get; }
}