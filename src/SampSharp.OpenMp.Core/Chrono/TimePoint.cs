using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TimePoint
{
    public readonly Nanoseconds Value;

    public static Nanoseconds operator -(TimePoint lhs, TimePoint rhs)
    {
        var nanos = rhs.Value.Value - lhs.Value.Value;
        return new Nanoseconds(nanos);
    }

    private TimePoint(long value)
    {
        Value = new Nanoseconds(value);
    }

    public static TimePoint FromDateTimeOffset(DateTimeOffset time)
    {
        var ticksSinceEpoch = time.UtcTicks - DateTimeOffset.UnixEpoch.Ticks;
        return new TimePoint(ticksSinceEpoch);
    }

    public DateTimeOffset ToDateTimeOffset()
    {
        return new DateTimeOffset(Value.Value + DateTimeOffset.UnixEpoch.Ticks, TimeSpan.Zero);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}