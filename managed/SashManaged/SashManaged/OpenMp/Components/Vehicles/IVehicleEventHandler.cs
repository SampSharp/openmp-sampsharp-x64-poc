namespace SashManaged.OpenMp;

[OpenMpEventHandler]
public interface IVehicleEventHandler
{
    void OnVehicleStreamIn(IVehicle vehicle, IPlayer player) { }
    void OnVehicleStreamOut(IVehicle vehicle, IPlayer player) { }
    void OnVehicleDeath(IVehicle vehicle, IPlayer player) { }
    void OnPlayerEnterVehicle(IPlayer player, IVehicle vehicle, bool passenger) { }
    void OnPlayerExitVehicle(IPlayer player, IVehicle vehicle) { }
    void OnVehicleDamageStatusUpdate(IVehicle vehicle, IPlayer player) { }
    bool OnVehiclePaintJob(IPlayer player, IVehicle vehicle, int paintJob) { return true; }
    bool OnVehicleMod(IPlayer player, IVehicle vehicle, int component) { return true; }
    bool OnVehicleRespray(IPlayer player, IVehicle vehicle, int colour1, int colour2) { return true; }
    void OnEnterExitModShop(IPlayer player, bool enterexit, int interiorID) { }
    void OnVehicleSpawn(IVehicle vehicle) { }
    bool OnUnoccupiedVehicleUpdate(IVehicle vehicle, IPlayer player, UnoccupiedVehicleUpdate  updateData) { return true; }
    bool OnTrailerUpdate(IPlayer player, IVehicle trailer) { return true; }
    bool OnVehicleSirenStateChange(IPlayer player, IVehicle vehicle, byte sirenState) { return true; }
}