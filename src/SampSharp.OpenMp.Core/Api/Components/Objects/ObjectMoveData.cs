using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ObjectMoveData
{
    public readonly Vector3 TargetPos;
    public readonly Vector3 TargetRot;
    public readonly float Speed;

    public ObjectMoveData(Vector3 targetPos, Vector3 targetRot, float speed)
    {
        TargetPos = targetPos;
        TargetRot = targetRot;
        Speed = speed;
    }
}