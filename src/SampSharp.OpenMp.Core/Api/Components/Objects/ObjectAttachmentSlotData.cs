using System.Numerics;
using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ObjectAttachmentSlotData
{
    public readonly int Model;
    public readonly int Bone;
    public readonly Vector3 Offset;
    public readonly Vector3 Rotation;
    public readonly Vector3 Scale;
    public readonly Colour Colour1;
    public readonly Colour Colour2;
}