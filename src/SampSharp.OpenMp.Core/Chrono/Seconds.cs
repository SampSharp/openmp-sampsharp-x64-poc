using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Chrono;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Seconds(int value)
{
    public readonly int Value = value;

    public TimeSpan AsTimeSpan()
    {
        return TimeSpan.FromSeconds(Value);
    }

    public static implicit operator TimeSpan(Seconds seconds)
    {
        return seconds.AsTimeSpan();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}