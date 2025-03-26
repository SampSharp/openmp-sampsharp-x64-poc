using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.Chrono;
using SampSharp.OpenMp.Core.RobinHood;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IVehicle"/> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IVehicle
{
    public partial void SetSpawnData(ref VehicleSpawnData data);
    private partial void GetSpawnData(out VehicleSpawnData data);

    public VehicleSpawnData GetSpawnData()
    {
        GetSpawnData(out var data);
        return data;
    }

    public partial bool IsStreamedInForPlayer(IPlayer player);
    public partial void StreamInForPlayer(IPlayer player);
    public partial void StreamOutForPlayer(IPlayer player);
    public partial void SetColour(int col1, int col2);
    public partial void GetColour(out Pair<int, int> result);

    public (int, int) GetColour()
    {
        GetColour(out var result);
        return result;
    }

    public partial void SetHealth(float health);
    public partial float GetHealth();
    public partial bool UpdateFromDriverSync(ref VehicleDriverSyncPacket vehicleSync, IPlayer player);
    public partial bool UpdateFromPassengerSync(ref VehiclePassengerSyncPacket passengerSync, IPlayer player);
    public partial bool UpdateFromUnoccupied(ref VehicleUnoccupiedSyncPacket unoccupiedSync, IPlayer player);
    public partial bool UpdateFromTrailerSync(ref VehicleTrailerSyncPacket unoccupiedSync, IPlayer player);
    public partial FlatPtrHashSet<IPlayer> StreamedForPlayers();
    public partial IPlayer GetDriver();
    public partial FlatPtrHashSet<IPlayer> GetPassengers();
    public partial void SetPlate(string plate);
    public partial string GetPlate();
    public partial void SetDamageStatus(int panelStatus, int doorStatus, byte lightStatus, byte tyreStatus, IPlayer vehicleUpdater = default);
    public partial void GetDamageStatus(out int panelStatus, out int doorStatus, out int lightStatus, out int tyreStatus);
    public partial void SetPaintJob(int paintjob);
    public partial int GetPaintJob();
    public partial void AddComponent(int component);
    public partial int GetComponentInSlot(int slot);
    public partial void RemoveComponent(int component);
    public partial void PutPlayer(IPlayer player, int seatID);
    public partial void SetZAngle(float angle);
    public partial float GetZAngle();
    public partial void SetParams(ref VehicleParams parameters);
    public partial void SetParamsForPlayer(IPlayer player, ref VehicleParams parameters);
    private partial void GetParams(out VehicleParams parameters);

    public VehicleParams GetParams()
    {
        GetParams(out var parameters);
        return parameters;
    }

    public partial bool IsDead();
    public partial void Respawn();
    [return: MarshalUsing(typeof(SecondsMarshaller))]
    public partial TimeSpan GetRespawnDelay();
    public partial void SetRespawnDelay([MarshalUsing(typeof(SecondsMarshaller))]TimeSpan delay);
    public partial bool IsRespawning();
    public partial void SetInterior(int interiorID);
    public partial int GetInterior();
    public partial void AttachTrailer(IVehicle trailer);
    public partial void DetachTrailer();
    public partial bool IsTrailer();
    public partial IVehicle GetTrailer();
    public partial IVehicle GetCab();
    public partial void Repair();
    public partial void AddCarriage(IVehicle carriage, int pos);
    public partial void UpdateCarriage(Vector3 pos, Vector3 veloc);
    public partial  ref CarriagesArray GetCarriages();
    public partial void SetVelocity(Vector3 velocity);
    public partial Vector3 GetVelocity();
    public partial void SetAngularVelocity(Vector3 velocity);
    public partial Vector3 GetAngularVelocity();
    public partial int GetModel();
    public partial byte GetLandingGearState();
    public partial bool HasBeenOccupied();
    public partial ref TimePoint GetLastOccupiedTime();
    public partial ref TimePoint GetLastSpawnTime();
    public partial bool IsOccupied();
    public partial void SetSiren(bool status);
    public partial byte GetSirenState();
    public partial uint GetHydraThrustAngle();
    public partial float GetTrainSpeed();
    public partial int GetLastDriverPoolID();
}