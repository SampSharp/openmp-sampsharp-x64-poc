using SampSharp.OpenMp.Core.Api;
using INPC = SampSharp.OpenMp.Core.Api.NPCs.INPC;
using INPCComponent = SampSharp.OpenMp.Core.Api.NPCs.INPCComponent;
using INPCEventHandler = SampSharp.OpenMp.Core.Api.NPCs.INPCEventHandler;

namespace SampSharp.Entities.SAMP;

internal class NpcSystem : DisposableSystem, INPCEventHandler
{
    private readonly IOmpEntityProvider _entityProvider;
    private readonly IEventDispatcher _eventDispatcher;

    public NpcSystem(IOmpEntityProvider entityProvider, IEventDispatcher eventDispatcher, SampSharpEnvironment omp)
    {
        _entityProvider = entityProvider;
        _eventDispatcher = eventDispatcher;

        // NPCs.dll may not be loaded in every server setup. Skip the handler
        // registration silently — none of the OnNPC* events will fire, and
        // INPCComponent calls from user code will simply no-op
        var npcs = INPCComponent.FromIComponent(omp.Components.QueryComponent(INPCComponent.ComponentId));
        if (!npcs.HasValue)
        {
            return;
        }

        AddDisposable(npcs.GetEventDispatcher().Add(this));
    }

    public void OnNPCFinishMove(INPC npc)
    {
        _eventDispatcher.Invoke("OnNPCFinishMove", _entityProvider.GetEntity(npc));
    }

    public void OnNPCCreate(INPC npc)
    {
        _eventDispatcher.Invoke("OnNPCCreate", _entityProvider.GetEntity(npc));
    }

    public void OnNPCDestroy(INPC npc)
    {
        _eventDispatcher.Invoke("OnNPCDestroy", _entityProvider.GetEntity(npc));
    }

    public void OnNPCSpawn(INPC npc)
    {
        _eventDispatcher.Invoke("OnNPCSpawn", _entityProvider.GetEntity(npc));
    }

    public void OnNPCRespawn(INPC npc)
    {
        _eventDispatcher.Invoke("OnNPCRespawn", _entityProvider.GetEntity(npc));
    }

    public void OnNPCDeath(INPC npc, IPlayer killer, int reason)
    {
        _eventDispatcher.Invoke("OnNPCDeath",
            _entityProvider.GetEntity(npc),
            _entityProvider.GetEntity(killer),
            reason);
    }
}