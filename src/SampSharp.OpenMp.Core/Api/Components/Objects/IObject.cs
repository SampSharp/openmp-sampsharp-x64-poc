using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IObject" /> interface.
/// </summary>
[OpenMpApi(typeof(IBaseObject))]
public readonly partial struct IObject
{
    public partial void AttachToPlayer(IPlayer player, Vector3 offset, Vector3 rotation);
    public partial void AttachToObject(IObject objekt, Vector3 offset, Vector3 rotation, bool syncRotation);
}