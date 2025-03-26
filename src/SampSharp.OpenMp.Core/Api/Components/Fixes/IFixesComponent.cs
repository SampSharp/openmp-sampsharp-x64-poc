using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.Chrono;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IFixesComponent"/> interface.
/// </summary>
[OpenMpApi(typeof(IComponent))]
public readonly partial struct IFixesComponent
{
    public static UID ComponentId => new(0xb5c615eff0329ff7);

    public partial bool SendGameTextToAll(string message, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan time, int style);
    public partial bool HideGameTextForAll(int style);
    public partial void ClearAnimation(IPlayer player, IActor actor);
}