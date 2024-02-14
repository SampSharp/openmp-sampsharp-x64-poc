using System.Runtime.InteropServices;

namespace SashManaged.Chrono;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TimePoint
{
    public readonly Nanoseconds Value;

    public static Nanoseconds operator -(TimePoint lhs, TimePoint rhs)
    {
        var nanos = rhs.Value.Value - lhs.Value.Value;
        return new Nanoseconds(nanos);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}