using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct CarriagesArray
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = OpenMpConstants.MAX_VEHICLE_CARRIAGES)]
    public readonly IVehicle[] Values;
}