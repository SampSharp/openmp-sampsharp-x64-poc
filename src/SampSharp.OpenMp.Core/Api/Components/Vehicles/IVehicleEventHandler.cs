﻿namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IVehiclesComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IVehicleEventHandler
{
    void OnVehicleStreamIn(IVehicle vehicle, IPlayer player);
    void OnVehicleStreamOut(IVehicle vehicle, IPlayer player);
    void OnVehicleDeath(IVehicle vehicle, IPlayer player);
    void OnPlayerEnterVehicle(IPlayer player, IVehicle vehicle, bool passenger);
    void OnPlayerExitVehicle(IPlayer player, IVehicle vehicle);
    void OnVehicleDamageStatusUpdate(IVehicle vehicle, IPlayer player);
    bool OnVehiclePaintJob(IPlayer player, IVehicle vehicle, int paintJob);
    bool OnVehicleMod(IPlayer player, IVehicle vehicle, int component);
    bool OnVehicleRespray(IPlayer player, IVehicle vehicle, int colour1, int colour2);
    void OnEnterExitModShop(IPlayer player, bool enterexit, int interiorId);
    void OnVehicleSpawn(IVehicle vehicle);
    bool OnUnoccupiedVehicleUpdate(IVehicle vehicle, IPlayer player, UnoccupiedVehicleUpdate updateData);
    bool OnTrailerUpdate(IPlayer player, IVehicle trailer);
    bool OnVehicleSirenStateChange(IPlayer player, IVehicle vehicle, byte sirenState);
}