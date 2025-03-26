namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="ITextLabel"/> interface.
/// </summary>
[OpenMpApi(typeof(ITextLabelBase))]
public readonly partial struct ITextLabel
{
    public partial bool IsStreamedInForPlayer(IPlayer player);
    public partial void StreamInForPlayer(IPlayer player);
    public partial void StreamOutForPlayer(IPlayer player);
}