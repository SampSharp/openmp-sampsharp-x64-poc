namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IActor
{
    public partial void SetSkin(int id);
    public partial int GetSkin();
    public partial void ApplyAnimation(ref AnimationData animation);
    public partial ref AnimationData GetAnimation();
    public partial void ClearAnimations();
    public partial void SetHealth(float health);
    public partial float GetHealth();
    public partial void SetInvulnerable(bool invuln);
    public partial bool IsInvulnerable();
    public partial bool IsStreamedInForPlayer(IPlayer player);
    public partial void StreamInForPlayer(IPlayer player);
    public partial void StreamOutForPlayer(IPlayer player);
    public partial ref ActorSpawnData GetSpawnData();
}