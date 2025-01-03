using System.Numerics;
using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ObjectAttachmentData
{
    public readonly AttachmentType Type;
    public readonly byte SyncRotation;
    public readonly int Id;
    public readonly Vector3 Offset;
    public readonly Vector3 Rotation;
}