﻿using System.Net;
using System.Numerics;
using JetBrains.Annotations;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

public class Player : WorldEntity
{
    private readonly IOmpEntityProvider _entityProvider;
    private readonly IPlayer _player;

    /// <summary>Constructs an instance of Player, should be used internally.</summary>
    protected Player(IOmpEntityProvider entityProvider, IPlayer player) : base((IEntity)player)
    {
        _entityProvider = entityProvider;
        _player = player;
    }

    /// <summary>
    /// Gets a value indicating whether the open.mp entity counterpart has been destroyed.
    /// </summary>
    protected bool IsOmpEntityDestroyed => _player.TryGetExtension<ComponentExtension>()?.IsOmpEntityDestroyed ?? true;

    /// <summary>Gets the name of this player.</summary>
    public virtual string Name
    {
        get => _player.GetName();
        [Obsolete("Use SetName(string) instead")]set => SetName(value);
    }

    /// <summary>
    /// Sets the name of this player.
    /// </summary>
    /// <param name="name">The name to be set.</param>
    /// <exception cref="InvalidPlayerNameException">Thrown when the name is invalid of already in use.</exception>
    public virtual void SetName(string name)
    {
        var result = _player.SetName(name);
        switch (result)
        {
            case EPlayerNameStatus.Invalid:
                throw new InvalidPlayerNameException("Player name is invalid");
            case EPlayerNameStatus.Taken:
                throw new InvalidPlayerNameException("Player name is already in use");
        }
    }

    /// <summary>Gets or sets the facing angle of this player.</summary>
    public virtual float Angle
    {
        get
        {
            var rotation = _player.GetRotation();

            // TODO implement
            throw new NotImplementedException();
        }
        set => throw new NotImplementedException();
    }

    /// <summary>Gets or sets the interior of this player.</summary>
    public virtual int Interior
    {
        get => (int)_player.GetInterior();
        set => _player.SetInterior((uint)value);
    }
    
    /// <summary>Gets or sets the health of this player.</summary>
    public virtual float Health
    {
        get => _player.GetHealth();
        set => _player.SetHealth(value);
    }

    /// <summary>Gets or sets the armor of this player.</summary>
    public virtual float Armour
    {
        get => _player.GetArmour();
        set => _player.SetArmour(value);
    }

    /// <summary>Gets the ammo of the Weapon this player is currently holding.</summary>
    public virtual int WeaponAmmo => _player.GetArmedWeaponAmmo();

    /// <summary>Gets the WeaponState of the Weapon this player is currently holding.</summary>
    public virtual WeaponState WeaponState => (WeaponState)_player.GetAimData().weaponState;// TODO: ref return?

    /// <summary>Gets the Weapon this player is currently holding.</summary>
    public virtual Weapon Weapon => (Weapon)_player.GetArmedWeapon();

    /// <summary>Gets the Player this player is aiming at.</summary>
    public virtual Player? TargetPlayer
    {
        get
        {
            var player = _player.GetTargetPlayer();
            if (!player.HasValue)
            {
                return null;
            }
            return _entityProvider.GetComponent(player);
        }
    }

    /// <summary>Gets or sets the team this player is in.</summary>
    public virtual int Team
    {
        get => _player.GetTeam();
        set => _player.SetTeam(value);
    }

    /// <summary>Gets or sets the score of this player.</summary>
    public virtual int Score
    {
        get => _player.GetScore();
        set => _player.SetScore(value);
    }

    /// <summary>Gets or sets the drunkenness level of this player.</summary>
    public virtual int DrunkLevel
    {
        get => _player.GetDrunkLevel();
        set => _player.SetDrunkLevel(value);
    }

    /// <summary>Gets or sets the Color of this player.</summary>
    public virtual Colour Color
    {
        get => _player.GetColour();
        set => _player.SetColour(value);
    }

    /// <summary>Gets or sets the skin of this player.</summary>
    public virtual int Skin
    {
        get => _player.GetSkin();
        set => _player.SetSkin(value);
    }

    /// <summary>Gets or sets the money of this player.</summary>
    public virtual int Money
    {
        get => _player.GetMoney();
        set => _player.SetMoney(value);
    }

    /// <summary>Gets the state of this player.</summary>
    public virtual PlayerState State => (PlayerState)_player.GetState();

    /// <summary>Gets the IP of this player.</summary>
    public virtual string Ip
    {
        get
        {
            _player.GetNetworkData();//TODO: ref return?
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets the ping of this player.</summary>
    public virtual int Ping => throw new NotImplementedException();

    /// <summary>Gets or sets the wanted level of this player.</summary>
    public virtual int WantedLevel
    {
        get => (int)_player.GetWantedLevel();
        set => _player.SetWantedLevel((uint)value);
    }

    /// <summary>Gets or sets the FightStyle of this player.</summary>
    public virtual FightStyle FightStyle
    {
        get => (FightStyle)_player.GetFightingStyle();
        set => _player.SetFightingStyle((PlayerFightingStyle)value);
    }

    /// <summary>Gets or sets the velocity of this player.</summary>
    public virtual Vector3 Velocity
    {
        get => _player.GetVelocity();
        set => _player.SetVelocity(value);
    }

    /// <summary>Gets the vehicle seat this player sits on.</summary>
    public virtual int VehicleSeat
    {
        get
        {
            var data = _player.QueryExtension<IPlayerVehicleData>();

            if (data == null)
            {
                return -1;
            }

            return data.GetSeat();
        }
    }

    /// <summary>Gets the index of the animation this player is playing.</summary>
    public virtual int AnimationIndex => _player.GetAnimationData().ID; // TODO: ref return?

    /// <summary>Gets or sets the SpecialAction of this player.</summary>
    public virtual SpecialAction SpecialAction
    {
        get => (SpecialAction)_player.GetAction();
        set => _player.SetAction((PlayerSpecialAction)value);
    }

    /// <summary>Gets or sets the position of the camera of this player.</summary>
    public virtual Vector3 CameraPosition
    {
        get => _player.GetCameraPosition();
        set => _player.SetCameraPosition(value);
    }

    /// <summary>Gets the front Vector3 of this player's camera.</summary>
    public virtual Vector3 CameraFrontVector => _player.GetAimData().camFrontVector; //  TODO: ref return?

    /// <summary>Gets the mode of this player's camera.</summary>
    public virtual CameraMode CameraMode => (CameraMode)_player.GetAimData().camMode; //  TODO: ref return?

    /// <summary>Gets the Actor this player is aiming at.</summary>
    public virtual EntityId TargetActor
    {
        get
        {
            // TODO: actor
            _player.GetTargetActor();
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets the GlobalObject the camera of this player is pointing at.</summary>
    public virtual EntityId CameraTargetGlobalObject
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets the PlayerObject the camera of this player is pointing at.</summary>
    public virtual EntityId CameraTargetPlayerObject
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets the GtaVehicle the camera of this player is pointing at.</summary>
    public virtual Vehicle? CameraTargetVehicle
    {
        get
        {
            var v = _player.GetCameraTargetVehicle();
            return v == null ? null : _entityProvider.GetComponent(v);
        }
    }

    /// <summary>Gets the GtaPlayer the camera of this player is pointing at.</summary>
    public virtual Player? CameraTargetPlayer
    {
        get
        {
            var p = _player.GetCameraTargetPlayer();
            return p == null ? null : _entityProvider.GetComponent(p);
        }
    }

    /// <summary>Gets the GtaPlayer the camera of this player is pointing at.</summary>
    public virtual EntityId CameraTargetActor
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets the entity (player, player object, object, vehicle or actor) the camera of this player is pointing at.</summary>
    public virtual EntityId CameraTargetEntity
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets whether this player is currently in any vehicle.</summary>
    public virtual bool InAnyVehicle => Vehicle != null;

    /// <summary>Gets whether this player is in his checkpoint.</summary>
    public virtual bool InCheckpoint
    {
        get
        {
            var data = _player.QueryExtension<IPlayerCheckpointData>();
            return data.Handle != 0 && data.GetCheckpoint().IsPlayerInside();
        }
    }

    /// <summary>Gets whether this player is in his race-checkpoint.</summary>
    public virtual bool InRaceCheckpoint   
    {
        get
        {
            var data = _player.QueryExtension<IPlayerCheckpointData>();
            return data.Handle != 0 && data.GetRaceCheckpoint().IsPlayerInside();
        }
    }

    /// <summary>Gets the component (object or vehicle) that this player is surfing.</summary>
    public virtual Component? SurfingEntity
    {
        get
        {
            var surf = _player.GetSurfingData();
            switch (surf.type)
            {
                case PlayerSurfingData.Type.Vehicle:
                    return _entityProvider.GetVehicle(surf.ID);
                case PlayerSurfingData.Type.Object:
                    throw new NotImplementedException();
                case PlayerSurfingData.Type.PlayerObject:
                    throw new NotImplementedException();
                default:
                    return null;
            }
        }
    }

    /// <summary>Gets the Vehicle this player is currently in.</summary>
    public virtual Vehicle? Vehicle
    {
        get
        {
            var data =_player.QueryExtension<IPlayerVehicleData>();
            return data == null ? null : _entityProvider.GetComponent(data.GetVehicle());
        }
    }

    /// <summary>Gets the Menu this player is currently in.</summary>
    public virtual Menu? Menu
    {
        get
        {
            var data = _player.QueryExtension<IPlayerMenuData>();
            var menuId = data.GetMenuID();
            return menuId == OpenMpConstants.INVALID_MENU_ID ? null :  _entityProvider.GetMenu(menuId);
        }
    }

    /// <summary>Gets or sets the rotation of this player.</summary>
    /// <remarks>Only the Z angle can be set!</remarks>
    public virtual Vector3 Rotation
    {
        // TODO: quat to euler
        get => new(0, 0, Angle);
        set => Angle = value.Z;
    }

    /// <summary>Gets whether this player is an actual player or an NPC.</summary>
    public virtual bool IsNpc => _player.IsBot();

    /// <summary>Gets whether this player is logged into RCON.</summary>
    public virtual bool IsAdmin
    {
        get
        {
            var data = _player.QueryExtension<IPlayerConsoleData>();
            if (data == null)
            {
                return false;
            }

            return data.HasConsoleAccess();
        }
    }

    private static readonly PlayerState[] _deadStates = [PlayerState.None, PlayerState.Spectating, PlayerState.Wasted];

    /// <summary>Gets a value indicating whether this player is alive.</summary>
    public virtual bool IsAlive => !_deadStates.Contains(State);

    /// <summary>Gets the end point (IP and port) of this player.</summary>
    public virtual IPEndPoint EndPoint
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Gets this player's game version.</summary>
    public virtual string Version =>
        // TODO: more client ver info
        _player.GetClientVersionName();

    /// <summary>Gets this player's global computer identifier string.</summary>
    public virtual string Gpci => _player.GetSerial();

    /// <summary>Gets a value indicating whether this player is selecting a textdraw.</summary>
    public virtual bool IsSelectingTextDraw { get; private set; }

    /// <summary>Gets the amount of time (in milliseconds) that a player has been connected to the server for.</summary>
    public virtual int ConnectedTime => throw new NotImplementedException();

    /// <summary>Gets the number of messages the server has received from the player.</summary>
    public virtual int MessagesReceived =>  throw new NotImplementedException();

    /// <summary>Gets the number of messages the player has received in the last second.</summary>
    public virtual int MessagesReceivedPerSecond => throw new NotImplementedException();

    /// <summary>Gets the number of messages the server has sent to the player.</summary>
    public virtual int MessagesSent => throw new NotImplementedException();

    /// <summary>Get the amount of information (in bytes) that the server has sent to the player.</summary>
    public virtual int BytesReceived => throw new NotImplementedException();

    /// <summary>Get the amount of information (in bytes) that the server has received from the player.</summary>
    public virtual int BytesSent => throw new NotImplementedException();

    /// <summary>Gets the packet loss percentage of a player. Packet loss means data the player is sending to the server is being lost (or vice-versa).</summary>
    /// <remarks>
    /// This value has been found to be currently unreliable. The value is not as expected when compared to the client, therefore this function should not be
    /// used as a packet loss kicker.
    /// </remarks>
    [Obsolete("This value is unreliable. See remarks for details.")]
    public virtual float PacketLossPercent => throw new NotImplementedException();

    /// <summary>Get a player's connection status.</summary>
    public virtual ConnectionStatus ConnectionStatus => throw new NotImplementedException();

    /// <summary>Gets the aspect ratio of this player's camera.</summary>
    public virtual float AspectCameraRatio => _player.GetAimData().aspectRatio;

    /// <summary>Gets the game camera zoom level for this player.</summary>
    public virtual float CameraZoom => _player.GetAimData().camZoom;

    /// <summary>
    /// This function can be used to change the spawn information of a specific player. It allows you to automatically set someone's spawn weapons, their
    /// team, skin and spawn position, normally used in case of mini games or automatic-spawn systems.
    /// </summary>
    /// <param name="team">The Team-ID of the chosen player.</param>
    /// <param name="skin">The skin which the player will spawn with.</param>
    /// <param name="position">The player's spawn position.</param>
    /// <param name="rotation">The direction in which the player needs to be facing after spawning.</param>
    /// <param name="weapon1">The first spawn-weapon for the player.</param>
    /// <param name="weapon1Ammo">The amount of ammunition for the primary spawn-weapon.</param>
    /// <param name="weapon2">The second spawn-weapon for the player.</param>
    /// <param name="weapon2Ammo">The amount of ammunition for the second spawn-weapon.</param>
    /// <param name="weapon3">The third spawn-weapon for the player.</param>
    /// <param name="weapon3Ammo">The amount of ammunition for the third spawn-weapon.</param>
    public virtual void SetSpawnInfo(int team, int skin, Vector3 position, float rotation, Weapon weapon1 = Weapon.None, int weapon1Ammo = 0,
        Weapon weapon2 = Weapon.None, int weapon2Ammo = 0, Weapon weapon3 = Weapon.None, int weapon3Ammo = 0)
    {
        var data = _player.QueryExtension<IPlayerClassData>();

        if (data == null)
        {
            throw new InvalidOperationException("No spawn data available.");
        }

        throw new NotImplementedException();
    }

    /// <summary>(Re)Spawns a player.</summary>
    public virtual void Spawn()
    {
        _player.Spawn();
    }

    /// <summary>Restore the camera to a place behind the player, after using a function like <see cref="CameraPosition" />.</summary>
    public virtual void PutCameraBehindPlayer()
    {
        _player.SetCameraBehind();
    }

    /// <summary>This sets this player's position then adjusts the Player's z-coordinate to the nearest solid ground under the position.</summary>
    /// <param name="position">The position to move this player to.</param>
    public virtual void SetPositionFindZ(Vector3 position)
    {
        _player.SetPositionFindZ(position);
    }

    /// <summary>Check if this player is in range of a point.</summary>
    /// <param name="range">The furthest distance the player can be from the point to be in range.</param>
    /// <param name="point">The point to check the range to.</param>
    /// <returns>True if this player is in range of the point, otherwise False.</returns>
    public virtual bool IsInRangeOfPoint(float range, Vector3 point)
    {
        return GetDistanceFromPoint(point) <= range;
    }

    /// <summary>Calculate the distance between this player and a map coordinate.</summary>
    /// <param name="point">The point to calculate the distance from.</param>
    /// <returns>The distance between the player and the point as a float.</returns>
    public virtual float GetDistanceFromPoint(Vector3 point)
    {
        var offset = point - Position;
        return offset.Length();
    }

    /// <summary>Checks if the specified <paramref name="player" /> is streamed in this player's client.</summary>
    /// <remarks>Players aren't streamed in on their own client, so if this player is the same as the other Player, it will return false!</remarks>
    /// <remarks>Players stream out if they are more than 150 meters away (see server.cfg - stream_distance)</remarks>
    /// <param name="player">The player to check is streamed in.</param>
    /// <returns>True if the other Player is streamed in for this player, False if not.</returns>
    public virtual bool IsPlayerStreamedIn(Player player)
    {
        return _player.IsStreamedInForPlayer(player);
    }

    /// <summary>Set the ammo of this player's weapon.</summary>
    /// <param name="weapon">The weapon to set the ammo of.</param>
    /// <param name="ammo">The amount of ammo to set.</param>
    public virtual void SetAmmo(Weapon weapon, int ammo)
    {
        _player.SetWeaponAmmo(new WeaponSlotData((byte)weapon, ammo));
    }

    /// <summary>Give this player a <see cref="Weapon" /> with a specified amount of ammo.</summary>
    /// <param name="weapon">The Weapon to give to this player.</param>
    /// <param name="ammo">The amount of ammo to give to this player.</param>
    public virtual void GiveWeapon(Weapon weapon, int ammo)
    {
        _player.GiveWeapon(new WeaponSlotData((byte)weapon, ammo));
    }


    /// <summary>Removes all weapons from this player.</summary>
    public virtual void ResetWeapons()
    {
        _player.ResetWeapons();
    }

    /// <summary>Sets the armed weapon of this player.</summary>
    /// <param name="weapon">The weapon that the player should be armed with.</param>
    public virtual void SetArmedWeapon(Weapon weapon)
    {
        _player.SetArmedWeapon((int)weapon);
    }

    /// <summary>Get the <see cref="Weapon" /> and ammo in this player's weapon slot.</summary>
    /// <param name="slot">The weapon slot to get data for (0-12).</param>
    /// <param name="weapon">The variable in which to store the weapon, passed by reference.</param>
    /// <param name="ammo">The variable in which to store the ammo, passed by reference.</param>
    public virtual void GetWeaponData(int slot, out Weapon weapon, out int ammo)
    {
        var data = _player.GetWeaponSlot(slot);
        weapon = (Weapon)data.Id;
        ammo = data.Ammo;
    }

    /// <summary>Give money to this player.</summary>
    /// <param name="money">The amount of money to give this player. Use a minus value to take money.</param>
    public virtual void GiveMoney(int money)
    {
        _player.GiveMoney(money);
    }

    /// <summary>Reset this player's money to $0.</summary>
    public virtual void ResetMoney()
    {
        _player.ResetMoney();
    }

    /// <summary>Check which keys this player is pressing.</summary>
    /// <remarks>
    /// Only the FUNCTION of keys can be detected; not actual keys. You can not detect if the player presses space, but you can detect if they press sprint
    /// (which can be mapped (assigned) to ANY key, but is space by default)).
    /// </remarks>
    /// <param name="keys">A set of bits containing this player's key states</param>
    /// <param name="upDown">Up or Down value, passed by reference.</param>
    /// <param name="leftRight">Left or Right value, passed by reference.</param>
    public virtual void GetKeys(out Keys keys, out int upDown, out int leftRight)
    {
        var data = _player.GetKeyData(); //TODO; ref return?
        keys = (Keys)data.keys;
        upDown = data.upDown;
        leftRight = data.leftRight;
    }

    /// <summary>Sets the clock of this player to a specific value. This also changes the daytime. (night/day etc.)</summary>
    /// <param name="hour">Hour to set (0-23).</param>
    /// <param name="minutes">Minutes to set (0-59).</param>
    public virtual void SetTime(int hour, int minutes)
    {
        _player.SetTime(hour, minutes);
    }

    /// <summary>Get this player's current game time. Set by <see cref="IServerService.SetWorldTime" />, or by <see cref="ToggleClock" />.</summary>
    /// <param name="hour">The variable to store the hour in, passed by reference.</param>
    /// <param name="minutes">The variable to store the minutes in, passed by reference.</param>
    public virtual void GetTime(out int hour, out int minutes)
    {
        _player.GetTime(out var hm);
        hour = hm.First;
        minutes = hm.Second;
    }

    /// <summary>Show/Hide the in-game clock (top right corner) for this player.</summary>
    /// <remarks>Time is not synced with other players!</remarks>
    /// <param name="toggle">True to show, False to hide.</param>
    public virtual void ToggleClock(bool toggle)
    {
        _player.UseClock(toggle);
    }

    /// <summary>
    /// Set this player's weather. If <see cref="ToggleClock" /> has been used to enable the clock, weather changes will interpolate (gradually change),
    /// otherwise will change instantly.
    /// </summary>
    /// <param name="weather">The weather to set.</param>
    public virtual void SetWeather(int weather)
    {
        _player.SetWeather(weather);
    }

    /// <summary>Forces this player to go back to class selection.</summary>
    /// <remarks>The player will not return to class selection until they re-spawn. This can be achieved with <see cref="ToggleSpectating" /></remarks>
    public virtual void ForceClassSelection()
    {
        _player.ForceClassSelection();
    }

    /// <summary>Display the cursor and allow this player to select a text draw.</summary>
    /// <param name="hoverColor">The color of the text draw when hovering over with mouse.</param>
    public virtual void SelectTextDraw(Colour hoverColor)
    {
        IsSelectingTextDraw = true;
        throw new NotImplementedException();
    }

    /// <summary>Cancel text draw selection with the mouse for this player.</summary>
    public virtual void CancelSelectTextDraw()
    {
        IsSelectingTextDraw = false;
        throw new NotImplementedException();
    }

    /// <summary>This function plays a crime report for this player - just like in single-player when CJ commits a crime.</summary>
    /// <param name="suspect">The suspect player which will be described in the crime report.</param>
    /// <param name="crime">The crime ID, which will be reported as a 10-code (i.e. 10-16 if 16 was passed as the crime ID).</param>
    public virtual void PlayCrimeReport(Player suspect, int crime)
    {
        _player.PlayerCrimeReport(suspect, crime);
    }

    /// <summary>Play an 'audio stream' for this player. Normal audio files also work (e.g. MP3).</summary>
    /// <param name="url">The url to play. Valid formats are mp3 and ogg/vorbis. A link to a .pls (playlist) file will play that playlist.</param>
    /// <param name="position">The position at which to play the audio.</param>
    /// <param name="distance">The distance over which the audio will be heard.</param>
    public virtual void PlayAudioStream(string url, Vector3 position, float distance)
    {
        _player.PlayAudio(url, true, position, distance);
    }

    /// <summary>Play an 'audio stream' for this player. Normal audio files also work (e.g. MP3).</summary>
    /// <param name="url">The url to play. Valid formats are mp3 and ogg/vorbis. A link to a .pls (playlist) file will play that playlist.</param>
    public virtual void PlayAudioStream(string url)
    {
        _player.PlayAudio(url);
    }

    /// <summary>Allows you to disable collisions between vehicles for a player.</summary>
    /// <param name="disable">if set to <c>true</c> disables the collision between vehicles.</param>
    public virtual void DisableRemoteVehicleCollisions(bool disable)
    {
        _player.SetRemoteVehicleCollisions(!disable);
    }

    /// <summary>Toggles camera targeting functions for a player.</summary>
    /// <param name="enable">if set to <c>true</c> the functionality is enabled.</param>
    public virtual void EnablePlayerCameraTarget(bool enable)
    {
        _player.UseCameraTargeting(enable);
    }

    /// <summary>Stops the current audio stream for this player.</summary>
    public virtual void StopAudioStream()
    {
        _player.StopAudio();
    }

    /// <summary>Loads or unloads an interior script for this player. (for example the Ammunation menu)</summary>
    /// <param name="shopName">The name of the shop, see <see cref="ShopName" /> for shop names.</param>
    public virtual void SetShopName(string shopName)
    {
        _player.SetShopName(shopName);
    }

    /// <summary>Set the skill level of a certain weapon type for this player.</summary>
    /// <remarks>The skill parameter is NOT the weapon ID, it is the skill type.</remarks>
    /// <param name="skill">The <see cref="WeaponSkill" /> you want to set the skill of.</param>
    /// <param name="level">The skill level to set for that weapon, ranging from 0 to 999. (A level out of range will max it out)</param>
    public virtual void SetSkillLevel(WeaponSkill skill, int level)
    {
        _player.SetSkillLevel((PlayerWeaponSkill)skill, level);
    }

    /// <summary>Attach an object to a specific bone on this player.</summary>
    /// <param name="index">The index (slot) to assign the object to (0-9).</param>
    /// <param name="modelId">The model to attach.</param>
    /// <param name="bone">The bone to attach the object to.</param>
    /// <param name="offset">offset for the object position.</param>
    /// <param name="rotation">rotation of the object.</param>
    /// <param name="scale">scale of the object.</param>
    /// <param name="materialColor1">The first object color to set.</param>
    /// <param name="materialColor2">The second object color to set.</param>
    /// <returns>True on success, False otherwise.</returns>
    public virtual bool SetAttachedObject(int index, int modelId, Bone bone, Vector3 offset, Vector3 rotation, Vector3 scale, Colour materialColor1,
        Colour materialColor2)
    {
        throw new NotImplementedException();
    }

    /// <summary>Remove an attached object from this player.</summary>
    /// <param name="index">The index of the object to remove (set with <see cref="SetAttachedObject" />).</param>
    /// <returns>True on success, False otherwise.</returns>
    public virtual bool RemoveAttachedObject(int index)
    {
        throw new NotImplementedException();
    }

    /// <summary>Check if this player has an object attached in the specified index (slot).</summary>
    /// <param name="index">The index (slot) to check.</param>
    /// <returns>True if the slot is used, False otherwise.</returns>
    public virtual bool IsAttachedObjectSlotUsed(int index)
    {
        throw new NotImplementedException();
    }

    /// <summary>Enter edition mode for an attached object.</summary>
    /// <param name="index">The index (slot) of the attached object to edit.</param>
    /// <returns>True on success, False otherwise.</returns>
    public virtual bool DoEditAttachedObject(int index)
    {
        throw new NotImplementedException();
    }

    /// <summary>Creates a chat bubble above this player's name tag.</summary>
    /// <param name="text">The text to display.</param>
    /// <param name="color">The text color.</param>
    /// <param name="drawDistance">The distance from where players are able to see the chat bubble.</param>
    /// <param name="expireTime">The time in milliseconds the bubble should be displayed for.</param>
    [Obsolete("Use SetChatBubble(string,Colour,float,TimeSpan) instead")]
    public virtual void SetChatBubble(string text, Colour color, float drawDistance, int expireTime)
    {
        SetChatBubble(text, color, drawDistance, TimeSpan.FromMilliseconds(expireTime));
    }
    
    /// <summary>Creates a chat bubble above this player's name tag.</summary>
    /// <param name="text">The text to display.</param>
    /// <param name="color">The text color.</param>
    /// <param name="drawDistance">The distance from where players are able to see the chat bubble.</param>
    /// <param name="expireTime">The time the bubble should be displayed for.</param>
    public virtual void SetChatBubble(string text, Colour color, float drawDistance, TimeSpan expireTime)
    {
        _player.SetChatBubble(text, ref color, drawDistance, expireTime);
    }

    /// <summary>Puts this player in a vehicle.</summary>
    /// <param name="vehicle">The vehicle for the player to be put in.</param>
    /// <param name="seatId">The ID of the seat to put the player in.</param>
    public virtual void PutInVehicle(Vehicle vehicle, int seatId)
    {
        ((IVehicle)vehicle).PutPlayer(_player, seatId);
    }

    /// <summary>Puts this player in a vehicle as driver.</summary>
    /// <param name="vehicle">The vehicle for the player to be put in.</param>
    public virtual void PutInVehicle(Vehicle vehicle)
    {
        PutInVehicle(vehicle, 0);
    }

    /// <summary>Removes/ejects this player from his vehicle.</summary>
    /// <param name="force">Force the removal of the player.</param>
    /// <remarks>
    /// The exiting animation is not synced for other players. This function will not work when used in the OnPlayerEnterVehicle event, because the player
    /// isn't in the vehicle when the callback is called. Use the OnPlayerStateChanged event instead.
    /// </remarks>
    public virtual void RemoveFromVehicle(bool force = false)
    {
        _player.RemoveFromVehicle(force);
    }

    /// <summary>Toggles whether this player can control themselves, basically freezes them.</summary>
    /// <param name="toggle">False to freeze the player or True to unfreeze them.</param>
    public virtual void ToggleControllable(bool toggle)
    {
        _player.SetControllable(toggle);
    }

    /// <summary>Plays the specified sound for this player at a specific point.</summary>
    /// <param name="soundId">The sound to play.</param>
    /// <param name="point">Point for the sound to play at.</param>
    public virtual void PlaySound(int soundId, Vector3 point)
    {
        _player.PlaySound(soundId, point);
    }

    /// <summary>Plays the specified sound for this player.</summary>
    /// <param name="soundId">The sound to play.</param>
    public virtual void PlaySound(int soundId)
    {
        _player.PlaySound(soundId, new Vector3()); // TODO: this correct?
    }

    /// <summary>Apply an animation to this player.</summary>
    /// <remarks>
    /// The <paramref name="forceSync" /> parameter, in most cases is not needed since players sync animations themselves. The <paramref name="forceSync" />
    /// parameter can force all players who can see this player to play the animation regardless of whether the player is performing that animation. This is useful in
    /// circumstances where the player can't sync the animation themselves. For example, they may be paused.
    /// </remarks>
    /// <param name="animationLibrary">The name of the animation library in which the animation to apply is in.</param>
    /// <param name="animationName">The name of the animation, within the library specified.</param>
    /// <param name="fDelta">The speed to play the animation (use 4.1).</param>
    /// <param name="loop">Set to True for looping otherwise set to False for playing animation sequence only once.</param>
    /// <param name="lockX">
    /// Set to False to return player to original x position after animation is complete for moving animations. The opposite effect occurs if set
    /// to True.
    /// </param>
    /// <param name="lockY">
    /// Set to False to return player to original y position after animation is complete for moving animations. The opposite effect occurs if set
    /// to True.
    /// </param>
    /// <param name="freeze">Will freeze the player in position after the animation finishes.</param>
    /// <param name="time">Timer in milliseconds. For a never ending loop it should be 0.</param>
    /// <param name="forceSync">Set to <c>true</c> to force the player to sync animation with other players in all instances</param>
    public virtual void ApplyAnimation(string animationLibrary, string animationName, float fDelta, bool loop, bool lockX, bool lockY, bool freeze, int time,
        bool forceSync)
    {
        var anim = new AnimationData(fDelta, loop, lockX, lockY, freeze, (uint)time, animationLibrary, animationName);

        // TODO: other sync?
        _player.ApplyAnimation(anim, forceSync ? PlayerAnimationSyncType.Sync : PlayerAnimationSyncType.NoSync);
    }

    /// <summary>Apply an animation to this player.</summary>
    /// <param name="animationLibrary">The name of the animation library in which the animation to apply is in.</param>
    /// <param name="animationName">The name of the animation, within the library specified.</param>
    /// <param name="fDelta">The speed to play the animation (use 4.1).</param>
    /// <param name="loop">Set to True for looping otherwise set to False for playing animation sequence only once.</param>
    /// <param name="lockX">
    /// Set to False to return player to original x position after animation is complete for moving animations. The opposite effect occurs if set
    /// to True.
    /// </param>
    /// <param name="lockY">
    /// Set to False to return player to original y position after animation is complete for moving animations. The opposite effect occurs if set
    /// to True.
    /// </param>
    /// <param name="freeze">Will freeze the player in position after the animation finishes.</param>
    /// <param name="time">Timer in milliseconds. For a never ending loop it should be 0.</param>
    public virtual void ApplyAnimation(string animationLibrary, string animationName, float fDelta, bool loop, bool lockX, bool lockY, bool freeze, int time)
    {
        ApplyAnimation(animationLibrary, animationName, fDelta, loop, lockX, lockY, freeze, time, false);
    }

    /// <summary>Clears all animations for this player.</summary>
    /// <param name="forceSync">Specifies whether the animation should be shown to streamed in players.</param>
    public virtual void ClearAnimations(bool forceSync)
    {
        // TODO: other sync?
        _player.ClearAnimations(forceSync ? PlayerAnimationSyncType.Sync : PlayerAnimationSyncType.NoSync);
    }

    /// <summary>Clears all animations for this player.</summary>
    public virtual void ClearAnimations()
    {
        ClearAnimations(false);
    }

    /// <summary>Get the animation library/name this player is playing.</summary>
    /// <param name="animationLibrary">String variable that stores the animation library.</param>
    /// <param name="animationName">String variable that stores the animation name.</param>
    /// <returns>True on success, False otherwise.</returns>
    public virtual bool GetAnimationName(out string? animationLibrary, out string? animationName)
    {
        var anim = _player.GetAnimationData(); // TODO: ref return?
        var id = anim.ID;
        (animationLibrary, animationName) = Animation.GetAnmiation(id);
        return true;
    }

    /// <summary>Sets a checkpoint (red circle) for this player. Also shows a red blip on the radar.</summary>
    /// <remarks>
    /// Checkpoints created on server-created objects will appear down on the 'real' ground, but will still function correctly. There is no fix available for
    /// this issue. A pickup can be used instead.
    /// </remarks>
    /// <param name="point">The point to set the checkpoint at.</param>
    /// <param name="size">The size of the checkpoint.</param>
    public virtual void SetCheckpoint(Vector3 point, float size)
    {
        var data = _player.QueryExtension<IPlayerCheckpointData>();

        if (data == null)
        {
            throw new InvalidOperationException("Missing checkpoint data");
        }

        var cp = data.GetCheckpoint();
        cp.SetPosition(ref point);
        cp.SetRadius(size);
        cp.Enable();
    }

    /// <summary>Disable any initialized checkpoints for this player.</summary>
    public virtual void DisableCheckpoint()
    {
        var data = _player.QueryExtension<IPlayerCheckpointData>();

        if (data == null)
        {
            throw new InvalidOperationException("Missing checkpoint data");
        }

        data.GetCheckpoint().Disable();
    }

    /// <summary>Creates a race checkpoint. When this player enters it, EnterRaceCheckpoint event is called.</summary>
    /// <param name="type">Type of checkpoint.</param>
    /// <param name="point">The point to set the checkpoint at.</param>
    /// <param name="nextPosition">Coordinates of the next point, for the arrow facing direction.</param>
    /// <param name="size">Length (diameter) of the checkpoint</param>
    public virtual void SetRaceCheckpoint(CheckpointType type, Vector3 point, Vector3 nextPosition, float size)
    {
        var data = _player.QueryExtension<IPlayerCheckpointData>();

        if (data == null)
        {
            throw new InvalidOperationException("Missing checkpoint data");
        }

        var cp = data.GetRaceCheckpoint();
        cp.SetPosition(ref point);
        cp.SetType((RaceCheckpointType)type);
        cp.SetRadius(size);
        cp.SetNextPosition(ref nextPosition);
        cp.Enable();
    }

    /// <summary>Disable any initialized race checkpoints for this player.</summary>
    public virtual void DisableRaceCheckpoint()
    {
        var data = _player.QueryExtension<IPlayerCheckpointData>();

        if (data == null)
        {
            throw new InvalidOperationException("Missing checkpoint data");
        }

        data.GetRaceCheckpoint().Disable();
    }

    /// <summary>Set the world boundaries for this player - players can not go out of the boundaries.</summary>
    /// <remarks>You can reset the player world bounds by setting the parameters to 20000.0000, -20000.0000, 20000.0000, -20000.0000.</remarks>
    /// <param name="xMax">The maximum X coordinate the player can go to.</param>
    /// <param name="xMin">The minimum X coordinate the player can go to.</param>
    /// <param name="yMax">The maximum Y coordinate the player can go to.</param>
    /// <param name="yMin">The minimum Y coordinate the player can go to.</param>
    public virtual void SetWorldBounds(float xMax, float xMin, float yMax, float yMin)
    {
        // TODO? what is this a Vector4? that sucks. well, what are the 4 components?
        _player.SetWorldBounds(new Vector4(xMax, xMin, yMax, yMin));
    }

    /// <summary>Change the color of this player's name tag and radar blip for another Player.</summary>
    /// <param name="player">The player whose color will be changed.</param>
    /// <param name="color">New color.</param>
    public virtual void SetPlayerMarker(Player player, Colour color)
    {
        _player.SetOtherColour(player, color);
    }

    /// <summary>
    /// This functions allows you to toggle the drawing of player name tags, health bars and armor bars which display above their head. For use of a similar
    /// function like this on a global level, <see cref="IServerService.ShowNameTags" /> function.
    /// </summary>
    /// <remarks><see cref="IServerService.ShowNameTags" /> must be set to <c>true</c> to be able to show name tags with <see cref="ShowNameTagForPlayer" />.</remarks>
    /// <param name="player">The player whose name tag will be shown or hidden.</param>
    /// <param name="show">True to show name tag, False to hide name tag.</param>
    public virtual void ShowNameTagForPlayer(Player player, bool show)
    {
        _player.ToggleOtherNameTag(player, show);
    }

    /// <summary>Set the direction this player's camera looks at. To be used in combination with <see cref="CameraPosition" />.</summary>
    /// <param name="point">The coordinates for this player's camera to look at.</param>
    /// <param name="cut">The style the camera-position changes.</param>
    public virtual void SetCameraLookAt(Vector3 point, CameraCut cut)
    {
        _player.SetCameraLookAt(point, (int)cut);
    }

    /// <summary>Set the direction this player's camera looks at. To be used in combination with <see cref="CameraPosition" />.</summary>
    /// <param name="point">The coordinates for this player's camera to look at.</param>
    public virtual void SetCameraLookAt(Vector3 point)
    {
        SetCameraLookAt(point, CameraCut.Cut);
    }

    /// <summary>Move this player's camera from one position to another, within the set time.</summary>
    /// <param name="from">The position the camera should start to move from.</param>
    /// <param name="to">The position the camera should move to.</param>
    /// <param name="time">Time in milliseconds.</param>
    /// <param name="cut">The jump cut to use. Defaults to CameraCut.Cut. Set to CameraCut. Move for a smooth movement.</param>
    public virtual void InterpolateCameraPosition(Vector3 from, Vector3 to, int time, CameraCut cut)
    {
        _player.InterpolateCameraPosition(from, to, time,(PlayerCameraCutType) cut);
    }

    /// <summary>Interpolate this player's camera's 'look at' point between two coordinates with a set speed.</summary>
    /// <param name="from">The position the camera should start to move from.</param>
    /// <param name="to">The position the camera should move to.</param>
    /// <param name="time">Time in milliseconds to complete interpolation.</param>
    /// <param name="cut">The jump cut to use. Defaults to CameraCut.Cut (pointless). Set to CameraCut.Move for interpolation.</param>
    public virtual void InterpolateCameraLookAt(Vector3 from, Vector3 to, int time, CameraCut cut)
    {
        _player.InterpolateCameraLookAt(from, to, time, (PlayerCameraCutType)cut);
    }

    /// <summary>Checks if this player is in a specific vehicle.</summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <returns>True if player is in the vehicle; False otherwise.</returns>
    public virtual bool IsInVehicle(Vehicle vehicle)
    {
        return Vehicle == vehicle;
    }

    /// <summary>Toggle stunt bonuses for this player.</summary>
    /// <param name="enable">True to enable stunt bonuses, False to disable them.</param>
    public virtual void EnableStuntBonus(bool enable)
    {
        _player.UseStuntBonuses(enable);
    }

    /// <summary>Toggle this player's spectate mode.</summary>
    /// <remarks>When the spectating is turned off, OnPlayerSpawn will automatically be called.</remarks>
    /// <param name="toggle">True to enable spectating and False to disable.</param>
    public virtual void ToggleSpectating(bool toggle)
    {
        _player.SetSpectating(toggle);
    }

    /// <summary>Makes this player spectate (watch) another player.</summary>
    /// <remarks>Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectatePlayer(Player,SpectateMode)" />.</remarks>
    /// <param name="targetPlayer">The Player that should be spectated.</param>
    /// <param name="mode">The mode to spectate with.</param>
    public virtual void SpectatePlayer(Player targetPlayer, SpectateMode mode)
    {
        _player.SpectatePlayer(targetPlayer, (PlayerSpectateMode)mode);
    }

    /// <summary>Makes this player spectate (watch) another player.</summary>
    /// <remarks>Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectatePlayer(Player)" />.</remarks>
    /// <param name="targetPlayer">The Player that should be spectated.</param>
    public virtual void SpectatePlayer(Player targetPlayer)
    {
        SpectatePlayer(targetPlayer, SpectateMode.Normal);
    }

    /// <summary>Sets this player to spectate another vehicle, i.e. see what its driver sees.</summary>
    /// <remarks>Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectateVehicle(Vehicle,SpectateMode)" />.</remarks>
    /// <param name="targetVehicle">The vehicle to spectate.</param>
    /// <param name="mode">Spectate mode.</param>
    public virtual void SpectateVehicle(Vehicle targetVehicle, SpectateMode mode)
    {
        _player.SpectateVehicle(targetVehicle, (PlayerSpectateMode)mode);
    }

    /// <summary>Sets this player to spectate another vehicle, i.e. see what its driver sees.</summary>
    /// <remarks>Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectateVehicle(Vehicle)" />.</remarks>
    /// <param name="targetVehicle">The vehicle to spectate.</param>
    public virtual void SpectateVehicle(Vehicle targetVehicle)
    {
        SpectateVehicle(targetVehicle, SpectateMode.Normal);
    }

    /// <summary>Starts recording this player's movements to a file, which can then be reproduced by an NPC.</summary>
    /// <param name="recordingType">The type of recording.</param>
    /// <param name="recordingName">
    /// Name of the file which will hold the recorded data. It will be saved in the scriptfiles folder, with an automatically added .rec
    /// extension.
    /// </param>
    public virtual void StartRecordingPlayerData(PlayerRecordingType recordingType, string recordingName)
    {
        throw new NotImplementedException();
    }

    /// <summary>Stops all the recordings that had been started with <see cref="StartRecordingPlayerData" /> for this player.</summary>
    public virtual void StopRecordingPlayerData()
    {
        throw new NotImplementedException();
    }

    /// <summary>Retrieves the start and end (hit) position of the last bullet a player fired.</summary>
    /// <param name="origin">The origin.</param>
    /// <param name="hitPosition">The hit position.</param>
    public virtual void GetLastShot(out Vector3 origin, out Vector3 hitPosition)
    {
        var data = _player.GetBulletData();

        origin = data.origin;
        hitPosition = data.hitPos;
    }

    /// <summary>
    /// This function sends a message to this player with a chosen color in the chat. The whole line in the chat box will be in the set color unless color
    /// embedding is used.
    /// </summary>
    /// <param name="color">The color of the message.</param>
    /// <param name="message">The text that will be displayed.</param>
    public virtual void SendClientMessage(Colour color, string message)
    {
        if (message.Length > 144)
        {
            _player.SendClientMessage(ref color, message[..144]);
            SendClientMessage(color, message[144..]);
        }
        else
        {
            _player.SendClientMessage(ref color, message);
        }
    }

    /// <summary>
    /// This function sends a message to this player with a chosen color in the chat. The whole line in the chat box will be in the set color unless color
    /// embedding is used.
    /// </summary>
    /// <param name="color">The color of the message.</param>
    /// <param name="messageFormat">The composite format string of the text that will be displayed (max 144 characters).</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    [StringFormatMethod("messageFormat")]
    public virtual void SendClientMessage(Colour color, string messageFormat, params object[] args)
    {
        SendClientMessage(color, string.Format(messageFormat, args));
    }

    /// <summary>
    /// This function sends a message to this player in white in the chat. The whole line in the chat box will be in the set color unless color embedding is
    /// used.
    /// </summary>
    /// <param name="message">The text that will be displayed.</param>
    public virtual void SendClientMessage(string message)
    {
        SendClientMessage(SAMP.Color.White, message);
    }

    /// <summary>
    /// This function sends a message to this player in white in the chat. The whole line in the chat box will be in the set color unless color embedding is
    /// used.
    /// </summary>
    /// <param name="messageFormat">The composite format string of the text that will be displayed (max 144 characters).</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    [StringFormatMethod("messageFormat")]
    public virtual void SendClientMessage(string messageFormat, params object[] args)
    {
        SendClientMessage(SAMP.Color.White, string.Format(messageFormat, args));
    }

    /// <summary>Kicks this player from the server. They will have to quit the game and re-connect if they wish to continue playing.</summary>
    public virtual void Kick()
    {
        _player.Kick();
    }

    /// <summary>
    /// Ban this player. The ban will be IP-based, and be saved in the samp.ban file in the server's root directory. <see cref="Ban(string)" /> allows you to
    /// ban with a reason, while you can ban and unban IPs using the RCON banip and unbanip commands.
    /// </summary>
    public virtual void Ban()
    {
        Ban(string.Empty);
    }

    /// <summary>Ban this player with a reason.</summary>
    /// <param name="reason">The reason for the ban.</param>
    public virtual void Ban(string reason)
    {
        _player.Ban(reason);
    }

    /// <summary>
    /// Sends a message in the name the specified <paramref name="sender" /> to this player. The message will appear in the chat box but can only be seen by
    /// this player. The line will start with the the sender's name in their color, followed by the <paramref name="message" /> in white.
    /// </summary>
    /// <param name="sender">The player which has sent the message.</param>
    /// <param name="message">The message that will be sent.</param>
    public virtual void SendPlayerMessageToPlayer(EntityId sender, string message)
    {
        throw new NotImplementedException();
    }

    /// <summary>Shows 'game text' (on-screen text) for a certain length of time for this player.</summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="time">The duration of the text being shown in milliseconds.</param>
    /// <param name="style">The style of text to be displayed.</param>
    [Obsolete("Obsolete. Use GameText(string,TimeSpan,int) instead.")]
    public virtual void GameText(string text, int time, int style)
    {
        GameText(text, TimeSpan.FromMilliseconds(time), style);
    }
    
    /// <summary>Shows 'game text' (on-screen text) for a certain length of time for this player.</summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="time">The duration of the text being shown.</param>
    /// <param name="style">The style of text to be displayed.</param>
    public virtual void GameText(string text, TimeSpan time, int style)
    {
        _player.SendGameText(text, time, style);
    }

    /// <summary>
    /// Creates an explosion for this player. Only this player will see explosion and feel its effects. This is useful when you want to isolate explosions
    /// from other players or to make them only appear in specific virtual worlds.
    /// </summary>
    /// <param name="position">The position of the explosion.</param>
    /// <param name="type">The explosion type.</param>
    /// <param name="radius">The radius of the explosion.</param>
    public virtual void CreateExplosion(Vector3 position, ExplosionType type, float radius)
    {
        _player.CreateExplosion(position, (int)type, radius);
    }

    /// <summary>Adds a death to the kill feed on the right-hand side of the screen of this player.</summary>
    /// <param name="killer">The player that killer the <paramref name="player" />.</param>
    /// <param name="player">The player that has been killed.</param>
    /// <param name="weapon">The reason for this player's death.</param>
    public virtual void SendDeathMessage(Player killer, Player player, Weapon weapon)
    {
        _player.SendDeathMessage(player, killer, (int)weapon);
    }

    /// <summary>Attaches a player's camera to an object.</summary>
    /// <param name="object">The object to attach the camera to.</param>
    public virtual void AttachCameraToObject(EntityId @object)
    {
        throw new NotImplementedException();
    }

    /// <summary>Lets this player edit the specified <paramref name="object" />.</summary>
    /// <param name="object">The object to edit.</param>
    public virtual void Edit(EntityId @object)
    {
        throw new NotImplementedException();
    }

    /// <summary>Cancels object editing mode for this player.</summary>
    public virtual void CancelEdit()
    {
        throw new NotImplementedException();
    }

    /// <summary>Lets this player select an object.</summary>
    public virtual void Select()
    {
        throw new NotImplementedException();
    }

    /// <summary>Removes a standard San Andreas model for this player within a specified range.</summary>
    /// <param name="modelId">The model identifier.</param>
    /// <param name="position">The position at which to remove the model.</param>
    /// <param name="radius">The radius in which to remove the model.</param>
    public virtual void RemoveBuilding(int modelId, Vector3 position, float radius)
    {
        throw new NotImplementedException();
    }

    /// <summary>Place an icon/marker on this player's map. Can be used to mark locations such as banks and hospitals to players.</summary>
    /// <param name="iconId">The player's icon identifier, ranging from 0 to 99. This means there is a maximum of 100 map icons.</param>
    /// <param name="position">The position to place the map icon at.</param>
    /// <param name="type">The type of the marker.</param>
    /// <param name="color">The color of the marker.</param>
    /// <param name="style">The style of the marker.</param>
    public virtual void SetMapIcon(int iconId, Vector3 position, MapIcon type, Colour color, SAMP.MapIconType style)
    {
        _player.SetMapIcon(iconId, position, (int)type, color, (MapIconStyle)style);
    }

    /// <summary>Removes a map icon that was set earlier for this player.</summary>
    /// <param name="iconId">The player's icon identifier.</param>
    public virtual void RemoveMapIcon(int iconId)
    {
        _player.UnsetMapIcon(iconId);
    }
    
    protected override void OnDestroyComponent()
    {
        if (!IsOmpEntityDestroyed)
        {
            Kick();
        }
    }

    public override string ToString()
    {
        return $"(Id: {Id}, Name: {Name})";
    }
    
    public static implicit operator IPlayer(Player player)
    {
        return player._player;
    }
}
