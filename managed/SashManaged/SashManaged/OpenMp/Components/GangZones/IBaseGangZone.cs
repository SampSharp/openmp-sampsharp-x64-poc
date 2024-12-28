namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtensible), typeof(IIDProvider))]
public readonly partial struct IBaseGangZone
{
    public partial bool IsShownForPlayer(IPlayer player);
    public partial bool IsFlashingForPlayer(IPlayer player);
    public partial void ShowForPlayer(IPlayer player, ref Colour colour);
    public partial void HideForPlayer(IPlayer player);
    public partial void FlashForPlayer(IPlayer player, ref Colour colour);
    public partial void StopFlashForPlayer(IPlayer player);
    public partial GangZonePos GetPosition();
    public partial void SetPosition(ref GangZonePos position);

    public partial bool IsPlayerInside(IPlayer player);

    public partial FlatPtrHashSet<IPlayer> GetShownFor();
    public partial Colour GetFlashingColourForPlayer(IPlayer player);
    public partial Colour GetColourForPlayer(IPlayer player);
    public partial void SetLegacyPlayer(IPlayer player);
    public partial IPlayer GetLegacyPlayer();
}