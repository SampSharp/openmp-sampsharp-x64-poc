namespace SashManaged.OpenMp;

public interface IEventHandler<TEventHandler> where TEventHandler : class
{
    /// <summary>
    /// Gets the manager for the creation of native event handler handles.
    /// </summary>
    static abstract INativeEventHandlerManager<TEventHandler> Manager { get; }
}