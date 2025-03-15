namespace SampSharp.Entities;

internal sealed record MethodResult(bool Value)
{
    public static MethodResult True { get; } = new(true);
    public static MethodResult False { get; } = new(false);

    public static object From(bool value)
    {
        return value ? True : False;
    }
}