using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly ref struct PairBoolString
{
    public readonly bool First;
    public readonly StringView Second;
}