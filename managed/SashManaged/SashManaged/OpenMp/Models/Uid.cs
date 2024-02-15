using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UID(ulong data)
{
    private readonly ulong _data = data;
}