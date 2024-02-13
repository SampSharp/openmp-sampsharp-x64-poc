using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IPlayerPool
{
    private readonly nint _data;

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern IEventDispatcher_PlayerConnectEventHandler IPlayerPool_getPlayerConnectDispatcher(IPlayerPool pool);

    public IEventDispatcher_PlayerConnectEventHandler GetPlayerConnectDispatcher()
    {
        return IPlayerPool_getPlayerConnectDispatcher(this);
    }
}