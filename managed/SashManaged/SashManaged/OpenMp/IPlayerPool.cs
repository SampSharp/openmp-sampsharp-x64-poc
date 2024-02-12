using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IPlayerPool
{
    private readonly nint _data;

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern IEventDispatcherPlayerConnectEventHandler IPlayerPool_getPlayerConnectDispatcher(IPlayerPool pool);

    public IEventDispatcherPlayerConnectEventHandler GetPlayerConnectDispatcher()
    {
        return IPlayerPool_getPlayerConnectDispatcher(this);
    }
}