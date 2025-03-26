using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IVehiclesComponent"/> interface.
/// </summary>
[OpenMpApi(typeof(IPoolComponent<IVehicle>))]
public readonly partial struct IVehiclesComponent
{
    public static UID ComponentId => new(0x3f1f62ee9e22ab19);
    public partial ref VehicleModelsArray Models();

    public partial IVehicle Create(bool isStatic, int modelID, Vector3 position, float Z = 0.0f, int colour1 = -1, int colour2 = -1, int respawnDelay = -1, bool addSiren = false);
    public partial IEventDispatcher<IVehicleEventHandler> GetEventDispatcher();
}