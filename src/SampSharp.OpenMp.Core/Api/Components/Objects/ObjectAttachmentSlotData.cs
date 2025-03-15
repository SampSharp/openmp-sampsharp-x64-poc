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

    public ObjectAttachmentSlotData(int model, int bone, Vector3 offset, Vector3 rotation, Vector3 scale, Colour colour1, Colour colour2)
    {
        Model = model;
        Bone = bone;
        Offset = offset;
        Rotation = rotation;
        Scale = scale;
        Colour1 = colour1;
        Colour2 = colour2;
    }
}