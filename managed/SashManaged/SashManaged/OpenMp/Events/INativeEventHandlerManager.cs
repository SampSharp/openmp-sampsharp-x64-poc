namespace SashManaged.OpenMp;

public interface INativeEventHandlerManager<TEventHandler> where TEventHandler : class
{
    NativeEventHandler<TEventHandler> Get(TEventHandler handler);
}