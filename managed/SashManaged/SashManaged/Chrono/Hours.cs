using System.Runtime.InteropServices;

namespace SashManaged.Chrono;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Hours(int value)
{
    public readonly int Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromHours(Value);
    }

    public static implicit operator TimeSpan(Hours hours)
    {
        return hours.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}