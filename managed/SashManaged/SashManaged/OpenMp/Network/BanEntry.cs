using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct BanEntry(HybridString46 addressString, TimePoint time, HybridString25 name, HybridString32 reason)
{
    public readonly HybridString46 AddressString = addressString;
    public readonly TimePoint Time = time;
    public readonly HybridString25 Name = name;
    public readonly HybridString32 Reason = reason;
}