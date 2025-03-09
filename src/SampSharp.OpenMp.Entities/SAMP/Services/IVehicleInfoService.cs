using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

/// <summary>Provides functionality for getting information about vehicle models and components.</summary>
public interface IVehicleInfoService
{
    /// <summary>Gets the car mod type of the specified <paramref name="componentId" />.</summary>
    /// <param name="componentId">The identifier of the component.</param>
    /// <returns>The car mod type of the component.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified <paramref name="componentId" /> is invalid.</exception>
    CarModType GetComponentType(int componentId);

    /// <summary>Gets information of type specified by <paramref name="infoType" /> for the specified <paramref name="vehicleModel" />.</summary>
    /// <param name="vehicleModel">The model of the vehicle.</param>
    /// <param name="infoType">The type of information to get.</param>
    /// <returns>The information about the vehicle model.</returns>
    Vector3 GetModelInfo(VehicleModelType vehicleModel, VehicleModelInfoType infoType);


    public bool IsValidComponentForVehicle(VehicleModelType vehicleModel, int componentId);

    public (int, int, int, int) GetRandomVehicleColor(VehicleModelType vehicleModel);

    public Color GetColorFromVehicleColor(int vehicleColor, uint alpha = 0xff);

    public int GetPassengerSeatCount(VehicleModelType vehicleModel);
}