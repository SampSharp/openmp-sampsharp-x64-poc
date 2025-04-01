using SampSharp.OpenMp.Core.Std.Chrono;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IPlayerPool.GetPlayerUpdateDispatcher"/>.
/// </summary>
[OpenMpEventHandler]
public partial interface IPlayerUpdateEventHandler
{
    bool OnPlayerUpdate(IPlayer player, TimePoint now);
}