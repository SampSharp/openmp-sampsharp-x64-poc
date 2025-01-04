using System.Numerics;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct ITextLabelBase
{
    public partial void SetText(string text);
    public partial string GetText();
    public partial void SetColour(Colour colour);
    public partial Colour GetColour();
    public partial void SetDrawDistance(float dist);
    public partial float GetDrawDistance();
    public partial void AttachToPlayer(IPlayer player, Vector3 offset);
    public partial void AttachToVehicle(IVehicle vehicle, Vector3 offset);
    public partial ref TextLabelAttachmentData GetAttachmentData();
    public partial void DetachFromPlayer(Vector3 position);
    public partial void DetachFromVehicle(Vector3 position);
    public partial void SetTestLOS(bool status);
    public partial bool GetTestLOS();
    public partial void SetColourAndText(Colour colour, string text);
}