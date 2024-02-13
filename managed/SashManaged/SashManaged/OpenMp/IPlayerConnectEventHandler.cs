using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IPlayerConnectEventHandler
{
    void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port);
    void OnPlayerConnect(IPlayer player);
    void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason);
    void OnPlayerClientInit(IPlayer player);
}

// TODO: generate source
internal class IPlayerConnectEventHandler_Handler : BaseEventHandler<IPlayerConnectEventHandler>
{
    private IPlayerConnectEventHandler_Handler(IPlayerConnectEventHandler handler, nint handle) : base(handler, handle)
    {
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port)
    {
        Active?.OnIncomingConnection(player, ipAddress, port);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerConnect(IPlayer player)
    {
        Active?.OnPlayerConnect(player);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
        Active?.OnPlayerDisconnect(player, reason);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerClientInit(IPlayer player)
    {
        Active?.OnPlayerClientInit(player);
    }

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe nint PlayerConnectEventHandlerImpl_create(delegate* unmanaged[Stdcall]<IPlayer, StringView, ushort, void> a,
        delegate* unmanaged[Stdcall]<IPlayer, void> b, delegate* unmanaged[Stdcall]<IPlayer, PeerDisconnectReason, void> c, delegate* unmanaged[Stdcall]<IPlayer, void> d);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern void PlayerConnectEventHandlerImpl_delete(nint ptr);

    public static unsafe IPlayerConnectEventHandler_Handler Activate(IPlayerConnectEventHandler handler)
    {
        if (Active == handler)
        {
            return (IPlayerConnectEventHandler_Handler)ActiveHandler!;
        }

        ThrowIfActive();

        var handle = PlayerConnectEventHandlerImpl_create(&OnIncomingConnection, &OnPlayerConnect, &OnPlayerDisconnect, &OnPlayerClientInit);
        return new IPlayerConnectEventHandler_Handler(handler, handle);
    }

    protected override void Delete()
    {
        PlayerConnectEventHandlerImpl_delete(Handle);
    }
}