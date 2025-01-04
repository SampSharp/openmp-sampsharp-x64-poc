using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerSurfingData
{
    public enum Type
    {
        None,
        Vehicle,
        Object,
        PlayerObject
    }

    public readonly Type type;
    public readonly int ID;
    public readonly Vector3 offset;
}