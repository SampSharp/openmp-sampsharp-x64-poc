using System.Numerics;
using SashManaged.Chrono;
using SashManaged.RobinHood;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IVehicle
{
    public partial void SetSpawnData(ref VehicleSpawnData data);
    public partial ref VehicleSpawnData GetSpawnData();
    public partial bool IsStreamedInForPlayer(IPlayer player);
    public partial void StreamInForPlayer(IPlayer player);
    public partial void StreamOutForPlayer(IPlayer player);
    public partial void SetColour(int col1, int col2);
    public partial PairInt GetColour();
    public partial void SetHealth(float Health);

    public partial float GetHealth();

    // TODO: virtual bool updateFromDriverSync( VehicleDriverSyncPacket& vehicleSync, IPlayer player);
    // TODO: virtual bool updateFromPassengerSync( VehiclePassengerSyncPacket& passengerSync, IPlayer player);
    // TODO: virtual bool updateFromUnoccupied( VehicleUnoccupiedSyncPacket& unoccupiedSync, IPlayer player);
    // TODO: virtual bool updateFromTrailerSync( VehicleTrailerSyncPacket& unoccupiedSync, IPlayer player);
    public partial FlatPtrHashSet<IPlayer> StreamedForPlayers();
    public partial IPlayer getDriver();
    public partial FlatPtrHashSet<IPlayer> GetPassengers();
    public partial void SetPlate(StringView plate);
    public partial StringView GetPlate();
    public partial void SetDamageStatus(int PanelStatus, int DoorStatus, byte LightStatus, byte TyreStatus, IPlayer vehicleUpdater = default);
    public partial void GetDamageStatus(ref int PanelStatus, ref int DoorStatus, ref int LightStatus, ref int TyreStatus);
    public partial void SetPaintJob(int paintjob);
    public partial int GetPaintJob();
    public partial void AddComponent(int component);
    public partial int GetComponentInSlot(int slot);
    public partial void RemoveComponent(int component);
    public partial void PutPlayer(IPlayer player, int SeatID);
    public partial void SetZAngle(float angle);
    public partial float GetZAngle();
    public partial void SetParams(ref VehicleParams parms);
    public partial void SetParamsForPlayer(IPlayer player, ref VehicleParams parms);
    public partial ref VehicleParams GetParams();
    public partial bool IsDead();
    public partial void Respawn();
    public partial Seconds GetRespawnDelay();
    public partial void SetRespawnDelay(Seconds delay);
    public partial bool IsRespawning();
    public partial void SetInterior(int InteriorID);
    public partial int GetInterior();
    public partial void AttachTrailer(IVehicle trailer);
    public partial void DetachTrailer();
    public partial bool IsTrailer();
    public partial IVehicle GetTrailer();
    public partial IVehicle GetCab();
    public partial void Repair();
    public partial void AddCarriage(IVehicle carriage, int pos);

    public partial void UpdateCarriage(Vector3 pos, Vector3 veloc);

    // TODO: virtual  StaticArray<IVehicle*, MAX_VEHICLE_CARRIAGES>& getCarriages();
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