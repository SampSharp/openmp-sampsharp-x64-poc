using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Nanoseconds(long value)
{
    public readonly long Value = value;

    public TimeSpan ToTimeSpan()
    {
        // ReSharper disable once PossibleLossOfFraction
        return TimeSpan.FromMicroseconds(Value / 1000);
    }

    public static explicit operator TimeSpan(Nanoseconds microseconds)
    {
        return microseconds.ToTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}