using System.Runtime.InteropServices;

namespace SashManaged.Chrono;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Milliseconds(long value)
{
    public readonly long Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromMilliseconds(Value);
    }

    public static implicit operator TimeSpan(Milliseconds milliseconds)
    {
        return milliseconds.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}