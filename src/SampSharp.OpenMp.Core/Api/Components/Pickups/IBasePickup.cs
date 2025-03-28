using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IBasePickup" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IBasePickup
{
    public partial void SetType(byte type, bool update = true);

    [OpenMpApiFunction("getType")]
    public partial byte GetPickupType();
    public partial void SetPositionNoUpdate(Vector3 position);
    public partial void SetModel(int id, bool update = true);
    public partial int GetModel();
    public partial bool IsStreamedInForPlayer(IPlayer player);
    public partial void StreamInForPlayer(IPlayer player);
    public partial void StreamOutForPlayer(IPlayer player);
    public partial void SetPickupHiddenForPlayer(IPlayer player, bool hidden);
    public partial bool IsPickupHiddenForPlayer(IPlayer player);
    public partial void SetLegacyPlayer(IPlayer player);
    public partial IPlayer GetLegacyPlayer();
}