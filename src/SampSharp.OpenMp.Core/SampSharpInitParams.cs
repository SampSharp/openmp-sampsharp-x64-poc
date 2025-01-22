using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

[StructLayout(LayoutKind.Sequential)]
public readonly struct SampSharpInitParams
{
    public readonly ICore Core;
    public readonly IComponentList ComponentList;
    private readonly BlittableStructRef<SampSharpInfo> _info;
    public SampSharpInfo Info => _info.GetValue();
}