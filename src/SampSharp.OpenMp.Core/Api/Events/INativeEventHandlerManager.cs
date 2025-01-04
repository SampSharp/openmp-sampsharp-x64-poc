namespace SampSharp.OpenMp.Core.Api;

public interface INativeEventHandlerManager<TEventHandler> where TEventHandler : class
{
    NativeEventHandler<TEventHandler> Get(TEventHandler handler);
}