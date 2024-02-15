namespace SashManaged.OpenMp;

public enum EventPriority : sbyte
{
    Highest = sbyte.MinValue,
    FairlyHigh = Highest / 2,
    Default = 0,
    Lowest = sbyte.MaxValue,
    FairlyLow = Lowest / 2
};