using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IObjectsComponent
{
    public static UID ComponentId => new(0x59f8415f72da6160);

    public partial IEventDispatcher<IObjectEventHandler> GetEventDispatcher();
    public partial void SetDefaultCameraCollision(bool collision);
    public partial bool GetDefaultCameraCollision();
    public partial IObject Create(int modelID, Vector3 position, Vector3 rotation, float drawDist = 0);
}