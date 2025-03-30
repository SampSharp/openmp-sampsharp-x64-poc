using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

/// <summary>
/// Provides the parameters for initializing the SampSharp application as provided by the SampSharp open.mp component.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly ref struct SampSharpInitParams
{
    /// <summary>
    /// The open.mp core.
    /// </summary>
    public readonly ICore Core;

    /// <summary>
    /// The open.mp component list.
    /// </summary>
    public readonly IComponentList ComponentList;

    private readonly BlittableStructRef<SampSharpInfo> _info;

    /// <summary>
    /// Gets information about the SampSharp open.mp component.
    /// </summary>
    public SampSharpInfo Info => _info.Value;
}