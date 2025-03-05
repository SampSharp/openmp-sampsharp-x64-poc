using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

#pragma warning disable CA1401

public static class VehicleData
{
    [DllImport("SampSharp", EntryPoint = "vehicles_isValidComponentForVehicleModel")]
    public static extern bool IsValidComponentForVehicleModel(int vehicleModel, int componentId);


    [DllImport("SampSharp", EntryPoint = "vehicles_getVehicleComponentSlot")]
    public static extern int GetVehicleComponentSlot(int component);

    [DllImport("SampSharp", EntryPoint = "vehicles_getVehicleModelInfo")]
    public static extern bool GetVehicleModelInfo(int model, SampSharp.OpenMp.Core.Api.VehicleModelInfoType type, out Vector3 outInfo);

    [DllImport("SampSharp", EntryPoint = "vehicles_getRandomVehicleColour")]
    public static extern void GetRandomVehicleColour(int modelId, out int colour1, out int colour2, out int colour3, out int colour4);

    [DllImport("SampSharp", EntryPoint = "vehicles_carColourIndexToColour")]
    public static extern Colour CarColourIndexToColour(int index, uint alpha = 0xFF);

    [DllImport("SampSharp", EntryPoint = "vehicles_getVehiclePassengerSeats")]
    public static extern byte GetVehiclePassengerSeats(int model);

}
#pragma warning restore CA1401