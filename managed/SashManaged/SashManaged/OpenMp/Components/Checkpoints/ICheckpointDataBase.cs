using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi]
public readonly partial struct ICheckpointDataBase
{
    public partial Vector3 GetPosition();
    public partial void SetPosition(ref Vector3 position);
    public partial float EtRadius();
    public partial void SetRadius(float radius);
    public partial bool IsPlayerInside();
    public partial void SetPlayerInside(bool inside);
    public partial void Enable();
    public partial void Disable();
    public partial bool IsEnabled();
}