using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Chrono;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TimePoint
{
    public readonly long Value;
    
    private TimePoint(long value)
    {
        Value = value;
    }

    public static TimePoint FromDateTimeOffset(DateTimeOffset time)
    {
        var ticksSinceEpoch = time.UtcTicks - DateTimeOffset.UnixEpoch.Ticks;
        return new TimePoint(ticksSinceEpoch);
    }

    public DateTimeOffset ToDateTimeOffset()
    {
        return new DateTimeOffset(Value + DateTimeOffset.UnixEpoch.Ticks, TimeSpan.Zero);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}