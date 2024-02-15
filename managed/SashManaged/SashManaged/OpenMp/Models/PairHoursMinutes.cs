using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PairHoursMinutes
{
    public readonly Hours Hours;
    public readonly Minutes Minutes;
}