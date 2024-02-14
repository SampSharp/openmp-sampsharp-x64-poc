using SashManaged.Chrono;

namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IPlayer
{
    public partial void Kick();
    public partial void Ban(StringView reason);

    public partial bool IsBot();

    //public partial const PeerNetworkData& getNetworkData()    ;
    public partial uint GetPing();
    //bool sendPacket(Span<byte> data, int channel, bool dispatchEvents = true);
    //bool sendRPC(int id, Span<byte> data, int channel, bool dispatchEvents = true);

    //virtual void broadcastRPCToStreamed(int id, Span<byte> data, int channel, bool skipFrom = false)    ;
    //virtual void broadcastPacketToStreamed(Span<byte> data, int channel, bool skipFrom = true)    ;
    //virtual void broadcastSyncPacket(Span<byte> data, int channel)    ;

    public partial void Spawn();
    public partial ClientVersion GetClientVersion();
    public partial StringView GetClientVersionName();
    public partial void SetPositionFindZ(GtaVector3 pos);
    public partial void SetCameraPosition(GtaVector3 pos);
    public partial GtaVector3 GetCameraPosition();
    public partial void SetCameraLookAt(GtaVector3 pos, int cutType);
    public partial GtaVector3 GetCameraLookAt();
    public partial void SetCameraBehind();
    public partial void InterpolateCameraPosition(GtaVector3 from, GtaVector3 to, int time, PlayerCameraCutType cutType);
    public partial void InterpolateCameraLookAt(GtaVector3 from, GtaVector3 to, int time, PlayerCameraCutType cutType);
    public partial void AttachCameraToObject(IObject obj);

    [OpenMpApiOverload("_player")]
    public partial void AttachCameraToObject(IPlayerObject obj);

    public partial EPlayerNameStatus SetName(StringView name);
    public partial StringView GetName();
    public partial StringView GetSerial();
    public partial void GiveWeapon(WeaponSlotData weapon);
    public partial void RemoveWeapon(byte weapon);
    public partial void SetWeaponAmmo(WeaponSlotData data);

    [return: OpenMpApiMarshall]
    public partial WeaponSlots GetWeapons();

    public partial WeaponSlotData GetWeaponSlot(int slot);
    public partial void ResetWeapons();
    public partial void SetArmedWeapon(int weapon);
    public partial int GetArmedWeapon();
    public partial int GetArmedWeaponAmmo();
    public partial void SetShopName(StringView name);
    public partial StringView GetShopName();
    public partial void SetDrunkLevel(int level);
    public partial int GetDrunkLevel();
    public partial void SetColour(Colour colour);
    public partial ref Colour GetColour();
    public partial void SetOtherColour(IPlayer other, Colour colour);
    public partial bool GetOtherColour(IPlayer other, ref Colour colour);
    public partial void SetControllable(bool controllable);
    public partial bool GetControllable();
    public partial void SetSpectating(bool spectating);
    public partial void SetWantedLevel(uint level);
    public partial uint GetWantedLevel();
    public partial void PlaySound(int sound, GtaVector3 pos);
    public partial int LastPlayedSound();
    public partial void PlayAudio(StringView url, bool usePos = false, GtaVector3 pos = default, float distance = 0); // TODO: GtaVector3
    public partial bool PlayerCrimeReport(IPlayer suspect, int crime);
    public partial void StopAudio();
    public partial StringView LastPlayedAudio();
    public partial void CreateExplosion(GtaVector3 vec, int type, float radius);
    public partial void SendDeathMessage(IPlayer player, IPlayer killer, int weapon);
    public partial void SendEmptyDeathMessage();
    public partial void RemoveDefaultObjects(uint model, GtaVector3 pos, float radius);
    public partial void ForceClassSelection();
    public partial void SetMoney(int money);
    public partial void GiveMoney(int money);
    public partial void ResetMoney();
    public partial int GetMoney();
    public partial void SetMapIcon(int id, GtaVector3 pos, int type, Colour colour, MapIconStyle style);
    public partial void UnsetMapIcon(int id);
    public partial void UseStuntBonuses(bool enable);
    public partial void ToggleOtherNameTag(IPlayer other, bool toggle);
    public partial void SetTime(Hours hr, Minutes min);
    public partial PairHoursMinutes getTime();
    public partial void UseClock(bool enable);
    public partial bool HasClock();
    public partial void UseWidescreen(bool enable);
    public partial bool HasWidescreen();
    public partial void SetTransform(GTAQuat tm);
    public partial void SetHealth(float health);
    public partial float GetHealth();
    public partial void SetScore(int score);
    public partial int GetScore();
    public partial void SetArmour(float armour);
    public partial float GetArmour();
    public partial void SetGravity(float gravity);
    public partial float GetGravity();
    public partial void SetWorldTime(Hours time);
    public partial void ApplyAnimation([OpenMpApiMarshall] AnimationData animation, PlayerAnimationSyncType syncType);
    public partial void ClearAnimations(PlayerAnimationSyncType syncType);
    public partial PlayerAnimationData GetAnimationData();
    public partial PlayerSurfingData GetSurfingData();
    public partial void StreamInForPlayer(IPlayer other);
    public partial bool IsStreamedInForPlayer(IPlayer other);

    public partial void StreamOutForPlayer(IPlayer other);

    // public partial FlatPtrHashSet<IPlayer>& streamedForPlayers()    ;
    public partial PlayerState GetState();
    public partial void SetTeam(int team);
    public partial int GetTeam();
    public partial void SetSkin(int skin, bool send = true);
    public partial int GetSkin();
    public partial void SetChatBubble(StringView text, ref Colour colour, float drawDist, Milliseconds expire);
    public partial void SendClientMessage(ref Colour colour, StringView message);
    public partial void SendChatMessage(IPlayer sender, StringView message);
    public partial void SendCommand(StringView message);
    public partial void SendGameText(StringView message, Milliseconds time, int style);
    public partial void HideGameText(int style);
    public partial bool HasGameText(int style);
    public partial bool GetGameText(int style, ref StringView message, ref Milliseconds time, ref Milliseconds remaining);
    public partial void SetWeather(int weatherID);
    public partial int GetWeather();
    public partial void SetWorldBounds(GtaVector4 coords);
    public partial GtaVector4 GetWorldBounds();
    public partial void SetFightingStyle(PlayerFightingStyle style);
    public partial PlayerFightingStyle GetFightingStyle();
    public partial void SetSkillLevel(PlayerWeaponSkill skill, int level);
    public partial void SetAction(PlayerSpecialAction action);
    public partial PlayerSpecialAction GetAction();
    public partial void SetVelocity(GtaVector3 velocity);
    public partial GtaVector3 GetVelocity();
    public partial void SetInterior(uint interior);
    public partial uint GetInterior();

    public partial ref PlayerKeyData GetKeyData();

    // public partial StaticArray<uint16_t, NUM_SKILL_LEVELS>& getSkillLevels()    ;
    public partial PlayerAimData GetAimData();
    public partial ref PlayerBulletData GetBulletData();
    public partial void UseCameraTargeting(bool enable);
    public partial bool HasCameraTargeting();
    public partial void RemoveFromVehicle(bool force);
    public partial IPlayer GetCameraTargetPlayer();
    public partial IVehicle GetCameraTargetVehicle();
    public partial IObject GetCameraTargetObject();
    public partial IActor GetCameraTargetActor();
    public partial IPlayer GetTargetPlayer();
    public partial IActor GetTargetActor();
    public partial void SetRemoteVehicleCollisions(bool collide);
    public partial void SpectatePlayer(IPlayer target, PlayerSpectateMode mode);
    public partial void SpectateVehicle(IVehicle target, PlayerSpectateMode mode);
    public partial ref PlayerSpectateData GetSpectateData();
    public partial void SendClientCheck(int actionType, int address, int offset, int count);
    public partial void ToggleGhostMode(bool toggle);
    public partial bool IsGhostModeEnabled();
    public partial int GetDefaultObjectsRemoved();
    public partial bool GetKickStatus();
    public partial void ClearTasks(PlayerAnimationSyncType syncType);
    public partial void AllowWeapons(bool allow);
    public partial bool AreWeaponsAllowed();
    public partial void AllowTeleport(bool allow);
    public partial bool IsTeleportAllowed();
    public partial bool IsUsingOfficialClient();
}