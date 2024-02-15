using System.Runtime.InteropServices;

namespace SashManaged;

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