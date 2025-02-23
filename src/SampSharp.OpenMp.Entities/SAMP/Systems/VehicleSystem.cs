using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class VehicleSystem : ISystem, IVehicleEventHandler, IDisposable
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;
    private IDisposable? _handler;

    public VehicleSystem(IEventService eventService, IOmpEntityProvider entityProvider)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
    }
    
    [Event]
    public void OnGameModeInit(OpenMp omp)
    {
        _handler = omp.Components.QueryComponent<IVehiclesComponent>().GetEventDispatcher().Add(this);
    }

    public void OnVehicleStreamIn(IVehicle vehicle, IPlayer player)
    {
        _eventService.Invoke("OnVehicleStreamIn", 
            _entityProvider.GetEntity(vehicle), 
            _entityProvider.GetEntity(player));
    }

    public void OnVehicleStreamOut(IVehicle vehicle, IPlayer player)
    {
        _eventService.Invoke("OnVehicleStreamOut", 
            _entityProvider.GetEntity(vehicle), 
            _entityProvider.GetEntity(player));
    }

    public void OnVehicleDeath(IVehicle vehicle, IPlayer player)
    {
        _eventService.Invoke("OnVehicleDeath", 
            _entityProvider.GetEntity(vehicle), 
            _entityProvider.GetEntity(player));
    }

    public void OnPlayerEnterVehicle(IPlayer player, IVehicle vehicle, bool passenger)
    {
        _eventService.Invoke("OnPlayerEnterVehicle",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle),
            passenger);
    }

    public void OnPlayerExitVehicle(IPlayer player, IVehicle vehicle)
    {
        _eventService.Invoke("OnPlayerExitVehicle",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle));
    }

    public void OnVehicleDamageStatusUpdate(IVehicle vehicle, IPlayer player)
    {
        _eventService.Invoke("OnVehicleDamageStatusUpdate",
            _entityProvider.GetEntity(vehicle),
            _entityProvider.GetEntity(player));
    }

    public bool OnVehiclePaintJob(IPlayer player, IVehicle vehicle, int paintJob)
    {
        var result = _eventService.Invoke("OnVehiclePaintJob",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle),
            paintJob);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnVehicleMod(IPlayer player, IVehicle vehicle, int component)
    {
        var result = _eventService.Invoke("OnVehicleMod",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle),
            component);
        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnVehicleRespray(IPlayer player, IVehicle vehicle, int colour1, int colour2)
    {
        var result = _eventService.Invoke("OnVehicleRespray",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle),
            colour1,
            colour2);
        return EventHelper.IsSuccessResponse(result);
    }

    public void OnEnterExitModShop(IPlayer player, bool enterexit, int interiorId)
    {
        _eventService.Invoke("OnEnterExitModShop",
            _entityProvider.GetEntity(player),
            enterexit,
            interiorId);
    }

    public void OnVehicleSpawn(IVehicle vehicle)
    {
        _eventService.Invoke("OnVehicleSpawn", _entityProvider.GetEntity(vehicle));
    }

    public bool OnUnoccupiedVehicleUpdate(IVehicle vehicle, IPlayer player, UnoccupiedVehicleUpdate updateData)
    {
        var result = _eventService.Invoke("OnUnoccupiedVehicleUpdate",
            _entityProvider.GetEntity(vehicle),
            _entityProvider.GetEntity(player),
            updateData.position,
            updateData.velocity,
            (int)updateData.seat);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnTrailerUpdate(IPlayer player, IVehicle trailer)
    {
        var result = _eventService.Invoke("OnTrailerUpdate",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(trailer));
        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnVehicleSirenStateChange(IPlayer player, IVehicle vehicle, byte sirenState)
    {
        var result = _eventService.Invoke("OnVehicleSirenStateChange",
            _entityProvider.GetEntity(player),
            _entityProvider.GetEntity(vehicle),
            sirenState);
        return EventHelper.IsSuccessResponse(result);
    }

    public void Dispose()
    {
        _handler?.Dispose();
        _handler = null;
    }
}