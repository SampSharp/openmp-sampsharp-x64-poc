using System.Runtime.InteropServices;

namespace SashManaged;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Minutes(int value)
{
    public readonly int Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromMinutes(Value);
    }

    public static implicit operator TimeSpan(Minutes minutes)
    {
        return minutes.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}