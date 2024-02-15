using System.Runtime.InteropServices;

namespace SashManaged.OpenMp;

[StructLayout(LayoutKind.Sequential)]
public readonly struct SemanticVersion
{
    public readonly byte major;
    public readonly byte minor;
    public readonly byte patch;
    public readonly ushort prerel;

    public Version ToVersion()
    {
        return new Version(major, minor, patch, prerel);
    }

    public override string ToString()
    {
        return $"{major}.{minor}.{patch}.{prerel}";
    }
}