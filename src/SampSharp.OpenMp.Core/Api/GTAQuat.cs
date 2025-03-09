using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct GTAQuat
{
    public readonly float W;
    public readonly float X;
    public readonly float Y;
    public readonly float Z;

    public GTAQuat(float x, float y, float z, float w)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    public static implicit operator Quaternion(GTAQuat gtaQuat)
    {
        // GTA quaternions are fubar, correct the components in our coordinate space.
        return new Quaternion(-gtaQuat.X, -gtaQuat.Y, -gtaQuat.Z, gtaQuat.W);
    }

    public static implicit operator GTAQuat(Quaternion quat)
    {
        // GTA quaternions are fubar, correct the components in our coordinate space.
        return new GTAQuat(-quat.X, -quat.Y, -quat.Z, quat.W);
    }
}