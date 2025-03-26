namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IActor"/> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IActor
{
    public partial void SetSkin(int id);
    public partial int GetSkin();
    public partial void ApplyAnimation(AnimationData animation);
    public partial AnimationData? GetAnimation();
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