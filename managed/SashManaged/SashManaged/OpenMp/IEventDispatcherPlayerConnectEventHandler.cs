using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IEventDispatcherPlayerConnectEventHandler
{
    private readonly nint _data;

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool IEventDispatcher_PlayerConnectEventHandler_addEventHandler(IEventDispatcherPlayerConnectEventHandler dispatcher, nint handler, EventPriority priority);

    public bool AddEventHandler(nint handler, EventPriority priority = EventPriority.Default)
    {
        return IEventDispatcher_PlayerConnectEventHandler_addEventHandler(this, handler, priority);
    }
}