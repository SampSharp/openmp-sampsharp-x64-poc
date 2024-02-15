using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IVehiclesComponent
{
    public static UID ComponentId => new(0x3f1f62ee9e22ab19);
    // virtual StaticArray<byte, MAX_VEHICLE_MODELS>& models();

    public partial IVehicle Create(bool isStatic, int modelID, Vector3 position, float Z = 0.0f, int colour1 = -1, int colour2 = -1, int respawnDelay = -1, bool addSiren = false);
    public partial IEventDispatcher<IVehicleEventHandler> GetEventDispatcher();
}