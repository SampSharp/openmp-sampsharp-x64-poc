using System.Runtime.InteropServices;
using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PairHoursMinutes
{
    public readonly Hours Hours;
    public readonly Minutes Minutes;
}