using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

public interface IPoolEventHandler<T> : IEventHandler<IPoolEventHandler<T>> where T : unmanaged
{
    void OnPoolEntryCreated(T entry);
    void OnPoolEntryDestroyed(T entry);

    // [OpenMpEventHandler] does not support generic types - implement it manually

    static INativeEventHandlerManager<IPoolEventHandler<T>> IEventHandler<IPoolEventHandler<T>>.Manager => NativeEventHandlerManager.Instance;

    public class NativeEventHandlerManager : NativeEventHandlerManager<IPoolEventHandler<T>>
    {
        public static NativeEventHandlerManager Instance { get; } = new();

        protected override (nint, object) Create(IPoolEventHandler<T> handler)
        {
            Delegate onPoolEntryCreatedDelegate = (PoolDelegate)(h => handler.OnPoolEntryCreated(StructPointer.AsStruct<T>(h))),
                onPoolEntryDestroyedDelegate = (PoolDelegate)(h => handler.OnPoolEntryDestroyed(StructPointer.AsStruct<T>(h)));

            nint onPoolEntryCreatedPtr = Marshal.GetFunctionPointerForDelegate(onPoolEntryCreatedDelegate),
                onPoolEntryDestroyedPtr = Marshal.GetFunctionPointerForDelegate(onPoolEntryDestroyedDelegate);

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