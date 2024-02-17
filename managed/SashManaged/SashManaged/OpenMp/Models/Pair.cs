using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Pair<T1, T2> 
    where T1 : unmanaged 
    where T2 : unmanaged
{
    public readonly T1 First;
    public readonly T2 Second;

    public override string ToString()
    {
        return $"<{First}, {Second}>";
    }
}