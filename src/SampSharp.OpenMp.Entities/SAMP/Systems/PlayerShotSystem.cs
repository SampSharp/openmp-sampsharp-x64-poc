using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class PlayerShotSystem : DisposableSystem, IPlayerShotEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public PlayerShotSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp openMp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;

        AddDisposable(openMp.Core.GetPlayers().GetPlayerShotDispatcher().Add(this));
    }

    public bool OnPlayerShotMissed(IPlayer player, ref PlayerBulletData bulletData)
    {
        var result = _eventService.Invoke("OnPlayerShotMissed", _entityProvider.GetEntity(player), bulletData);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerShotPlayer(IPlayer player, IPlayer target, ref PlayerBulletData bulletData)
    {
        var result = _eventService.Invoke("OnPlayerShotPlayer", _entityProvider.GetEntity(player), _entityProvider.GetEntity(target), bulletData);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerShotVehicle(IPlayer player, IVehicle target, ref PlayerBulletData bulletData)
    {
        var result = _eventService.Invoke("OnPlayerShotVehicle", _entityProvider.GetEntity(player), _entityProvider.GetEntity(target), bulletData);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerShotObject(IPlayer player, IObject target, ref PlayerBulletData bulletData)
    {
        var result = _eventService.Invoke("OnPlayerShotObject", _entityProvider.GetEntity(player), _entityProvider.GetEntity(target), bulletData);

        return EventHelper.IsSuccessResponse(result);
    }

    public bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, ref PlayerBulletData bulletData)
    {
        var result = _eventService.Invoke("OnPlayerShotPlayerObject", _entityProvider.GetEntity(player), _entityProvider.GetEntity(target), bulletData);

        return EventHelper.IsSuccessResponse(result);
    }
}