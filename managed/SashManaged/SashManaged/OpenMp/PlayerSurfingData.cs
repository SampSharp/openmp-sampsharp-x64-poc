using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

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
    public readonly GtaVector3 offset;
}