using System.Numerics;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(ICheckpointDataBase))]
public readonly partial struct IRaceCheckpointData
{
    
    public new partial RaceCheckpointType GetType(); // TODO: name override
    public partial void SetType(RaceCheckpointType type);
    public partial Vector3 GetNextPosition();
    public partial void SetNextPosition(ref Vector3 nextPosition);
}