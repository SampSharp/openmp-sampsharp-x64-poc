using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IObjectEventHandler"/> interface.
/// </summary>
[OpenMpEventHandler]
public partial interface IObjectEventHandler
{
    void OnMoved(IObject objekt);
    void OnPlayerObjectMoved(IPlayer player, IPlayerObject objekt);
    void OnObjectSelected(IPlayer player, IObject objekt, int model, Vector3 position);
    void OnPlayerObjectSelected(IPlayer player, IPlayerObject objekt, int model, Vector3 position);
    void OnObjectEdited(IPlayer player, IObject objekt, ObjectEditResponse response, Vector3 offset, Vector3 rotation);
    void OnPlayerObjectEdited(IPlayer player, IPlayerObject objekt, ObjectEditResponse response, Vector3 offset, Vector3 rotation);
    void OnPlayerAttachedObjectEdited(IPlayer player, int index, bool saved, ref ObjectAttachmentSlotData data);
}