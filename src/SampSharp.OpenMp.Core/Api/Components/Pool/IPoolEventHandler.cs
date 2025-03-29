namespace SampSharp.OpenMp.Core.Api;

public interface IPoolEventHandler<T> : IEventHandler<IPoolEventHandler<T>> where T : unmanaged
{
    void OnPoolEntryCreated(T entry);
    void OnPoolEntryDestroyed(T entry);

    // [OpenMpEventHandler] does not support generic types - implement it manually

    static IEventHandlerMarshaller<IPoolEventHandler<T>> IEventHandler<IPoolEventHandler<T>>.Marshaller => NativeEventHandlerManager.Instance;

    public class NativeEventHandlerManager : EventHandlerMarshaller<IPoolEventHandler<T>>
    {
        public static NativeEventHandlerManager Instance { get; } = new();

        protected override (nint, object) Create(IPoolEventHandler<T> handler)
        {
            Delegate onPoolEntryCreatedDelegate = (PoolDelegate)(h => handler.OnPoolEntryCreated(StructPointer.AsStruct<T>(h))),
                onPoolEntryDestroyedDelegate = (PoolDelegate)(h => handler.OnPoolEntryDestroyed(StructPointer.AsStruct<T>(h)));

            nint onPoolEntryCreatedPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(onPoolEntryCreatedDelegate),
                onPoolEntryDestroyedPtr = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(onPoolEntryDestroyedDelegate);

            object[] data = [onPoolEntryCreatedDelegate, onPoolEntryDestroyedDelegate];

            var handle = PoolEventHandlerInterop.PoolEventHandlerImpl_create(onPoolEntryCreatedPtr, onPoolEntryDestroyedPtr);
            return (handle, data);
        }

        protected override void Free(nint handle)
        {
            PoolEventHandlerInterop.PoolEventHandlerImpl_delete(handle);
        }
    }
}