using System.Runtime.InteropServices;
using SashManaged.OpenMp;

// ReSharper disable InconsistentNaming
// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable SeparateLocalFunctionsWithJumpStatement

namespace SashManaged;

// [OpenMpEventHandler]
public partial interface IPlayerConnectEventHandler2 : IEventHandler<IPlayerConnectEventHandler2>
{
    void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port);
    void OnPlayerConnect(IPlayer player);
    void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason);
    void OnPlayerClientInit(IPlayer player);

    static INativeEventHandlerManager<IPlayerConnectEventHandler2> IEventHandler<IPlayerConnectEventHandler2>.Manager => NativeEventHandlerManager.Instance;

    public class NativeEventHandlerManager : NativeEventHandlerManager<IPlayerConnectEventHandler2>
    {
        public static NativeEventHandlerManager Instance { get; } = new();

        private delegate void OnIncomingConnection_(IPlayer player, StringView ipAddress, ushort port);
        private delegate void OnPlayerConnect_(IPlayer player);
        private delegate void OnPlayerDisconnect_(IPlayer player, PeerDisconnectReason reason);
        private delegate void OnPlayerClientInit_(IPlayer player);
        
        protected override (IntPtr, object) Create(IPlayerConnectEventHandler2 handler)
        {
            Delegate 
                __OnIncomingConnection_delegate = (OnIncomingConnection_)handler.OnIncomingConnection, 
                __OnPlayerConnect_delegate = (OnPlayerConnect_)handler.OnPlayerConnect, 
                __OnPlayerDisconnect_delegate = (OnPlayerDisconnect_)handler.OnPlayerDisconnect, 
                __OnPlayerClientInit_delegate = (OnPlayerClientInit_)handler.OnPlayerClientInit;

            nint __OnIncomingConnection_ptr = Marshal.GetFunctionPointerForDelegate(__OnIncomingConnection_delegate), 
                __OnPlayerConnect_ptr = Marshal.GetFunctionPointerForDelegate(__OnPlayerConnect_delegate), 
                __OnPlayerDisconnect_ptr = Marshal.GetFunctionPointerForDelegate(__OnPlayerDisconnect_delegate), 
                __OnPlayerClientInit_ptr = Marshal.GetFunctionPointerForDelegate(__OnPlayerClientInit_delegate);

            nint handle = __PInvoke(__OnIncomingConnection_ptr, __OnPlayerConnect_ptr, __OnPlayerDisconnect_ptr, __OnPlayerClientInit_ptr);

            object[] data = [__OnIncomingConnection_delegate, __OnPlayerConnect_delegate, __OnPlayerDisconnect_delegate, __OnPlayerClientInit_delegate];

            return (handle, data);

            // Local P/Invoke
            [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "PlayerConnectEventHandlerImpl_create", ExactSpelling = true)]
            static extern nint __PInvoke(nint _OnIncomingConnection, nint _OnPlayerConnect, nint _OnPlayerDisconnect, nint _OnPlayerClientInit);
        }

        protected override void Free(IntPtr handle)
        {
            __PInvoke(handle);
        
            // Local P/Invoke
            [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "PlayerConnectEventHandlerImpl_delete", ExactSpelling = true)]
            static extern void __PInvoke(nint ptr);
        }
    }
}