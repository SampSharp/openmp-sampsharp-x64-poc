using System.Runtime.InteropServices;
using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PairHoursMinutes
{
    // Pair<Hours, Minutes>
    public readonly Hours hours;
    public readonly Minutes minutes;
}