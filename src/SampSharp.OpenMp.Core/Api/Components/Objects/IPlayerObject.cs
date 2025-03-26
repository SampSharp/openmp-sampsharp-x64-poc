using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayerObject"/> interface.
/// </summary>
[OpenMpApi(typeof(IBaseObject))]
public readonly partial struct IPlayerObject
{
    public partial void AttachToObject(IPlayerObject objekt, Vector3 offset, Vector3 rotation);
    public partial void AttachToPlayer(IPlayer player, Vector3 offset, Vector3 rotation);
}