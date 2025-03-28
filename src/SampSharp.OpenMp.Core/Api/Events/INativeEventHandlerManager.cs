namespace SampSharp.OpenMp.Core.Api;

// TODO: rename to Marshaller
public interface INativeEventHandlerManager<TEventHandler> where TEventHandler : class
{
    NativeEventHandler<TEventHandler> Get(TEventHandler handler);
}