namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// Provides the events for <see cref="IClassesComponent.GetEventDispatcher" />.
/// </summary>
[OpenMpEventHandler]
public partial interface IClassEventHandler
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}