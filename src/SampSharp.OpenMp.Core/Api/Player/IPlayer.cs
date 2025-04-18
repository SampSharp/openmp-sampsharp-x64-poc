﻿using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.RobinHood;
using SampSharp.OpenMp.Core.Std;
using SampSharp.OpenMp.Core.Std.Chrono;

namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IPlayer" /> interface.
/// </summary>
[OpenMpApi(typeof(IExtensible), typeof(IEntity))]
public readonly partial struct IPlayer
{
    public partial void Kick();
    public partial void Ban(string reason);
    public partial bool IsBot();
    public partial BlittableStructRef<PeerNetworkData> GetNetworkData();
    public partial uint GetPing();
    public partial bool SendPacket(SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial bool SendRPC(int id, SpanLite<byte> data, int channel, bool dispatchEvents = true);
    public partial void BroadcastRPCToStreamed(int id, SpanLite<byte> data, int channel, bool skipFrom = false)    ;
    public partial void BroadcastPacketToStreamed(SpanLite<byte> data, int channel, bool skipFrom = true)    ;
    public partial void BroadcastSyncPacket(SpanLite<byte> data, int channel)    ;
    public partial void Spawn();
    public partial ClientVersion GetClientVersion();
    public partial string GetClientVersionName();
    public partial void SetPositionFindZ(Vector3 pos);
    public partial void SetCameraPosition(Vector3 pos);
    public partial Vector3 GetCameraPosition();
    public partial void SetCameraLookAt(Vector3 pos, int cutType);
    public partial Vector3 GetCameraLookAt();
    public partial void SetCameraBehind();
    public partial void InterpolateCameraPosition(Vector3 from, Vector3 to, int time, PlayerCameraCutType cutType);
    public partial void InterpolateCameraLookAt(Vector3 from, Vector3 to, int time, PlayerCameraCutType cutType);
    public partial void AttachCameraToObject(IObject obj);

    [OpenMpApiOverload("_player")]
    public partial void AttachCameraToObject(IPlayerObject obj);

    public partial EPlayerNameStatus SetName(string name);
    public partial string GetName();
    public partial string GetSerial();
    public partial void GiveWeapon(WeaponSlotData weapon);
    public partial void RemoveWeapon(byte weapon);
    public partial void SetWeaponAmmo(WeaponSlotData data);
    public partial BlittableStructRef<WeaponSlots> GetWeapons();
    private partial void GetWeaponSlot(int slot, out WeaponSlotData data);

    public WeaponSlotData GetWeaponSlot(int slot)
    {
        GetWeaponSlot(slot, out var data);
        return data;
    }

    public partial void ResetWeapons();
    public partial void SetArmedWeapon(int weapon);
    public partial int GetArmedWeapon();
    public partial int GetArmedWeaponAmmo();
    public partial void SetShopName(string name);
    public partial string GetShopName();
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
    public partial void PlaySound(int sound, Vector3 pos);
    public partial int LastPlayedSound();
    public partial void PlayAudio(string url, bool usePos = false, Vector3 pos = default, float distance = 0);
    public partial bool PlayerCrimeReport(IPlayer suspect, int crime);
    public partial void StopAudio();
    public partial string LastPlayedAudio();
    public partial void CreateExplosion(Vector3 vec, int type, float radius);
    public partial void SendDeathMessage(IPlayer player, IPlayer killer, int weapon);
    public partial void SendEmptyDeathMessage();
    public partial void RemoveDefaultObjects(uint model, Vector3 pos, float radius);
    public partial void ForceClassSelection();
    public partial void SetMoney(int money);
    public partial void GiveMoney(int money);
    public partial void ResetMoney();
    public partial int GetMoney();
    public partial void SetMapIcon(int id, Vector3 pos, int type, Colour colour, MapIconStyle style);
    public partial void UnsetMapIcon(int id);
    public partial void UseStuntBonuses(bool enable);
    public partial void ToggleOtherNameTag(IPlayer other, bool toggle);
    public partial void SetTime(int hr, int min);
    private partial void GetTime(out Pair<int, int> result);

    public (int hour, int minutes) GetTime()
    {
        GetTime(out var result);
        return result;
    }

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
    public partial void SetWorldTime(int time);
    public partial void ApplyAnimation(AnimationData animation, PlayerAnimationSyncType syncType);
    public partial void ClearAnimations(PlayerAnimationSyncType syncType);
    public partial PlayerAnimationData GetAnimationData();
    public partial PlayerSurfingData GetSurfingData();
    public partial void StreamInForPlayer(IPlayer other);
    public partial bool IsStreamedInForPlayer(IPlayer other);
    public partial void StreamOutForPlayer(IPlayer other);
    public partial FlatPtrHashSet<IPlayer> StreamedForPlayers();
    public partial PlayerState GetState();
    public partial void SetTeam(int team);
    public partial int GetTeam();
    public partial void SetSkin(int skin, bool send = true);
    public partial int GetSkin();
    public partial void SetChatBubble(string text, ref Colour colour, float drawDist, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan expire);
    public partial void SendClientMessage(ref Colour colour, string message);
    public partial void SendChatMessage(IPlayer sender, string message);
    public partial void SendCommand(string message);
    public partial void SendGameText(string message, [MarshalUsing(typeof(MillisecondsMarshaller))]TimeSpan time, int style);
    public partial void HideGameText(int style);
    public partial bool HasGameText(int style);
    public partial bool GetGameText(int style, out string? message, [MarshalUsing(typeof(MillisecondsMarshaller))]out TimeSpan time, [MarshalUsing(typeof(MillisecondsMarshaller))]out TimeSpan remaining);
    public partial void SetWeather(int weatherId);
    public partial int GetWeather();
    public partial void SetWorldBounds(Vector4 coords);
    public partial Vector4 GetWorldBounds();
    public partial void SetFightingStyle(PlayerFightingStyle style);
    public partial PlayerFightingStyle GetFightingStyle();
    public partial void SetSkillLevel(PlayerWeaponSkill skill, int level);
    public partial void SetAction(PlayerSpecialAction action);
    public partial PlayerSpecialAction GetAction();
    public partial void SetVelocity(Vector3 velocity);
    public partial Vector3 GetVelocity();
    public partial void SetInterior(uint interior);
    public partial uint GetInterior();
    public partial ref PlayerKeyData GetKeyData();
    public partial ref SkillsArray GetSkillLevels();
    public partial ref PlayerAimData GetAimData();
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