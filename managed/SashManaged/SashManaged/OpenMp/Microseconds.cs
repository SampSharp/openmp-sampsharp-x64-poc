using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Microseconds(long value)
{
    public readonly long Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromMicroseconds(Value);
    }

    public static implicit operator TimeSpan(Microseconds microseconds)
    {
        return microseconds.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct Hours(int value)
{
    public readonly int Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromHours(Value);
    }

    public static implicit operator TimeSpan(Hours microseconds)
    {
        return microseconds.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}