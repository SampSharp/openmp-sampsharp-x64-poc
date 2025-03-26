namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IClassEventHandler"/> interface.
/// </summary>
[OpenMpEventHandler]
public partial interface IClassEventHandler
{
    bool OnPlayerRequestClass(IPlayer player, uint classId);
}