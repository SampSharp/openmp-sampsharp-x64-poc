using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class ObjectSystem : DisposableSystem, IObjectEventHandler
{
    private readonly IEventService _eventService;
    private readonly IOmpEntityProvider _entityProvider;

    public ObjectSystem(IEventService eventService, IOmpEntityProvider entityProvider, OpenMp omp)
    {
        _eventService = eventService;
        _entityProvider = entityProvider;
        AddDisposable(omp.Components.QueryComponent<IObjectsComponent>().GetEventDispatcher().Add(this));
    }

    public void OnMoved(IObject objekt)
    {
        _eventService.Invoke("OnMoved", _entityProvider.GetEntity(objekt));
    }

    public void OnPlayerObjectMoved(IPlayer player, IPlayerObject objekt)
    {
        _eventService.Invoke("OnPlayerObjectMoved", _entityProvider.GetEntity(player), _entityProvider.GetEntity(objekt));
    }

    public void OnObjectSelected(IPlayer player, IObject objekt, int model, Vector3 position)
    {
        _eventService.Invoke("OnObjectSelected", _entityProvider.GetEntity(player), _entityProvider.GetEntity(objekt), model, position);
    }

    public void OnPlayerObjectSelected(IPlayer player, IPlayerObject objekt, int model, Vector3 position)
    {
        _eventService.Invoke("OnPlayerObjectSelected", _entityProvider.GetEntity(player), _entityProvider.GetEntity(objekt), model, position);
    }

    public void OnObjectEdited(IPlayer player, IObject objekt, ObjectEditResponse response, Vector3 offset, Vector3 rotation)
    {
        _eventService.Invoke("OnObjectEdited", _entityProvider.GetEntity(player), _entityProvider.GetEntity(objekt), response, offset, rotation);
    }

    public void OnPlayerObjectEdited(IPlayer player, IPlayerObject objekt, ObjectEditResponse response, Vector3 offset, Vector3 rotation)
    {
        _eventService.Invoke("OnPlayerObjectEdited", _entityProvider.GetEntity(player), _entityProvider.GetEntity(objekt), response, offset, rotation);
    }

    public void OnPlayerAttachedObjectEdited(IPlayer player, int index, bool saved, ref ObjectAttachmentSlotData data)
    {
        _eventService.Invoke("OnPlayerAttachedObjectEdited", _entityProvider.GetEntity(player), index, saved, data);
    }
}