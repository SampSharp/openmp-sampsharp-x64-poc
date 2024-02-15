using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct VehicleParams
{
    public readonly sbyte engine = -1;
    public readonly sbyte lights = -1;
    public readonly sbyte alarm = -1;
    public readonly sbyte doors = -1;
    public readonly sbyte bonnet = -1;
    public readonly sbyte boot = -1;
    public readonly sbyte objective = -1;
    public readonly sbyte siren = -1;
    public readonly sbyte doorDriver = -1;
    public readonly sbyte doorPassenger = -1;
    public readonly sbyte doorBackLeft = -1;
    public readonly sbyte doorBackRight = -1;
    public readonly sbyte windowDriver = -1;
    public readonly sbyte windowPassenger = -1;
    public readonly sbyte windowBackLeft = -1;
    public readonly sbyte windowBackRight = -1;

    public VehicleParams()
    {
    }

    public bool IsSet()
    {
        return engine != -1 || lights != -1 || alarm != -1 || doors != -1 || bonnet != -1 || boot != -1 || objective != -1 || siren != -1 || doorDriver != -1 ||
               doorPassenger != -1 || doorBackLeft != -1 || doorBackRight != -1 || windowDriver != -1 || windowPassenger != -1 || windowBackLeft != -1 || windowBackRight != -1;
    }
}