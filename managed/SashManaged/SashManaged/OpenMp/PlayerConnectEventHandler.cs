using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

public class PlayerConnectEventHandler : IDisposable
{
    private static IPlayerConnectEventHandler? _activeHandler;
    private static nint? _handle;

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port)
    {
        _activeHandler?.OnIncomingConnection(player, ipAddress, port);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerConnect(IPlayer player)
    {
        _activeHandler?.OnPlayerConnect(player);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
        _activeHandler?.OnPlayerDisconnect(player, reason);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void OnPlayerClientInit(IPlayer player)
    {
        _activeHandler?.OnPlayerClientInit(player);
    }

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe nint PlayerConnectEventHandlerImpl_create(
        delegate* unmanaged[Stdcall]<IPlayer, StringView, ushort, void> a,
        delegate* unmanaged[Stdcall]<IPlayer, void> b,
        delegate* unmanaged[Stdcall]<IPlayer, PeerDisconnectReason, void> c,
        delegate* unmanaged[Stdcall]<IPlayer, void> d
    );

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern void PlayerConnectEventHandlerImpl_delete(nint ptr);

    private PlayerConnectEventHandler() { }

    public static unsafe nint Activate(IPlayerConnectEventHandler handler)
    {
        if (_handle != null)
        {
            throw new InvalidOperationException("Already active");
        }

        _handle = PlayerConnectEventHandlerImpl_create(&OnIncomingConnection, &OnPlayerConnect, &OnPlayerDisconnect, &OnPlayerClientInit);
        _activeHandler = handler;

        return _handle.Value;
    }

    ~PlayerConnectEventHandler()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_handle != null)
        {
            PlayerConnectEventHandlerImpl_delete(_handle.Value);
            _handle = null;
            _activeHandler = null;
        }
    }
}