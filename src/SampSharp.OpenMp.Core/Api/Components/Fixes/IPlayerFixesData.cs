using System.Runtime.InteropServices.Marshalling;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtension))]
public readonly partial struct IPlayerFixesData
{
    public static UID ExtensionId => new(0x672d5d6fbb094ef7);

    public partial bool SendGameText(string message, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan time, int style);
    public partial bool HideGameText(int style);
    public partial bool HasGameText(int style);
    public partial bool GetGameText(int style, out string? message, [MarshalUsing(typeof(MillisecondsMarshaller))]out TimeSpan time, [MarshalUsing(typeof(MillisecondsMarshaller))]out TimeSpan remaining);
    public partial void ApplyAnimation(IPlayer player, IActor actor, AnimationData animation);
}