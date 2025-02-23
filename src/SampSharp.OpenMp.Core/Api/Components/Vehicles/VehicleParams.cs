using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct VehicleParams
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

    public VehicleParams(sbyte engine, sbyte lights, sbyte alarm, sbyte doors, sbyte bonnet, sbyte boot, sbyte objective, sbyte siren, sbyte doorDriver, sbyte doorPassenger, sbyte doorBackLeft, sbyte doorBackRight, sbyte windowDriver, sbyte windowPassenger, sbyte windowBackLeft, sbyte windowBackRight)
    {
        this.engine = engine;
        this.lights = lights;
        this.alarm = alarm;
        this.doors = doors;
        this.bonnet = bonnet;
        this.boot = boot;
        this.objective = objective;
        this.siren = siren;
        this.doorDriver = doorDriver;
        this.doorPassenger = doorPassenger;
        this.doorBackLeft = doorBackLeft;
        this.doorBackRight = doorBackRight;
        this.windowDriver = windowDriver;
        this.windowPassenger = windowPassenger;
        this.windowBackLeft = windowBackLeft;
        this.windowBackRight = windowBackRight;
    }

    public VehicleParams()
    {
    }

    public bool IsSet()
    {
        return engine != -1 || lights != -1 || alarm != -1 || doors != -1 || bonnet != -1 || boot != -1 || objective != -1 || siren != -1 || doorDriver != -1 ||
               doorPassenger != -1 || doorBackLeft != -1 || doorBackRight != -1 || windowDriver != -1 || windowPassenger != -1 || windowBackLeft != -1 || windowBackRight != -1;
    }
}