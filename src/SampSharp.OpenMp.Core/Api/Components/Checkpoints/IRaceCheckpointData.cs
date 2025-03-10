﻿using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(ICheckpointDataBase))]
public readonly partial struct IRaceCheckpointData
{
    [OpenMpApiFunction("getType")]
    public partial RaceCheckpointType GetCheckpointType();
    public partial void SetType(RaceCheckpointType type);
    public partial Vector3 GetNextPosition();
    public partial void SetNextPosition(ref Vector3 nextPosition);
}