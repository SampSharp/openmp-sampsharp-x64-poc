using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PlayerSpectateData
{
    public enum ESpectateType
    {
        None,
        Vehicle,
        Player
    }

    public readonly bool spectating;
    public readonly int spectateID;
    public readonly ESpectateType type;
}