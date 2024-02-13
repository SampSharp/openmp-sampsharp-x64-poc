using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct IEventDispatcher_PlayerConnectEventHandler
{
    private readonly nint _data;
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool IEventDispatcher_PlayerConnectEventHandler_addEventHandler(IEventDispatcher_PlayerConnectEventHandler dispatcher, nint handler, EventPriority priority);

    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool IEventDispatcher_PlayerConnectEventHandler_removeEventHandler(IEventDispatcher_PlayerConnectEventHandler dispatcher, nint handler);
    
    [DllImport("SampSharp", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool IEventDispatcher_PlayerConnectEventHandler_hasEventHandler(IEventDispatcher_PlayerConnectEventHandler dispatcher, nint handler, out EventPriority priority);
    
    public bool AddEventHandler(IPlayerConnectEventHandler handler, EventPriority priority = EventPriority.Default)
    {
        var active = IPlayerConnectEventHandler_Handler.Activate(handler);

        if (!IEventDispatcher_PlayerConnectEventHandler_addEventHandler(this, active.Handle, priority))
        {
            return false;
        }

        var dispatcher = this;
        active.Disposing += (sender, e) => dispatcher.RemoveEventHandler(handler);

        return true;
    }

    public bool RemoveEventHandler(IPlayerConnectEventHandler handler)
    {
        if (IPlayerConnectEventHandler_Handler.Active != handler)
        {
            return false;
        }

        return IEventDispatcher_PlayerConnectEventHandler_removeEventHandler(this, IPlayerConnectEventHandler_Handler.ActiveHandle!.Value);
    }

    public bool HasEventHandler(IPlayerConnectEventHandler handler, out EventPriority priority)
    {
        if (IPlayerConnectEventHandler_Handler.Active != handler)
        {
            priority = default;
            return false;
        }

        return IEventDispatcher_PlayerConnectEventHandler_hasEventHandler(this, IPlayerConnectEventHandler_Handler.ActiveHandle!.Value, out priority);
    }
}