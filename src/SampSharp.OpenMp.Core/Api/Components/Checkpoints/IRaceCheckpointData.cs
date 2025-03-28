using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IRaceCheckpointData" /> interface.
/// </summary>
[OpenMpApi(typeof(ICheckpointDataBase))]
public readonly partial struct IRaceCheckpointData
{
    [OpenMpApiFunction("getType")]
    public partial RaceCheckpointType GetCheckpointType();
    public partial void SetType(RaceCheckpointType type);
    public partial Vector3 GetNextPosition();
    public partial void SetNextPosition(ref Vector3 nextPosition);
}