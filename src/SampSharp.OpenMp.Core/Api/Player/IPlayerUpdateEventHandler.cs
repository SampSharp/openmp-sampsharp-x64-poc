using SampSharp.OpenMp.Core.Std.Chrono;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpEventHandler]
public partial interface IPlayerUpdateEventHandler
{
    bool OnPlayerUpdate(IPlayer player, TimePoint now);
}