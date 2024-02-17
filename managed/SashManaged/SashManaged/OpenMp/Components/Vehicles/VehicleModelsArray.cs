using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleModelsArray
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = OpenMpConstants.MAX_VEHICLE_MODELS)]
    public readonly byte[] Values;
}