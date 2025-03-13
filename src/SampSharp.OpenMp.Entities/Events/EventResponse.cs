namespace SampSharp.Entities;

public sealed record EventResponse(bool Value)
{
    public static EventResponse True { get; } = new(true);
    public static EventResponse False { get; } = new(false);
}