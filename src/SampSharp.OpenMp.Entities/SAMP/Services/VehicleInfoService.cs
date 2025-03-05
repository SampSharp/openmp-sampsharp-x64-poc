using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class VehicleInfoService : IVehicleInfoService
{
    public CarModType GetComponentType(int componentId)
    {
        return (CarModType)VehicleData.GetVehicleComponentSlot(componentId);
    }

    public bool IsValidComponentForVehicle(VehicleModelType vehicleModel, int componentId)
    {
        return VehicleData.IsValidComponentForVehicleModel((int)vehicleModel, componentId);
    }

    public Vector3 GetModelInfo(VehicleModelType vehicleModel, VehicleModelInfoType infoType)
    {
        VehicleData.GetVehicleModelInfo((int)vehicleModel, (SampSharp.OpenMp.Core.Api.VehicleModelInfoType)infoType, out var outInfo);
        return outInfo;
    }

    public (VehicleColor, VehicleColor, VehicleColor, VehicleColor) GetRandomVehicleColor(VehicleModelType vehicleModel)
    {
        VehicleData.GetRandomVehicleColour((int)vehicleModel, out var a, out var b, out var c, out var d);
        return ((VehicleColor)a, (VehicleColor)b, (VehicleColor)c, (VehicleColor)d);
    }

    public Colour GetColorFromVehicleColor(VehicleColor vehicleColor, uint alpha = 0xff)
    {
        return VehicleData.CarColourIndexToColour((int)vehicleColor, alpha);
    }

    public int GetPassengerSeatCount(VehicleModelType vehicleModel)
    {
        return VehicleData.GetVehiclePassengerSeats((int)vehicleModel);
    }
}