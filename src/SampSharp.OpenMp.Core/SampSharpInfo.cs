using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct SampSharpInfo
{
    public readonly Size Size;
    public readonly int ApiVersion;
    public readonly SemanticVersion Version;
}
