using System.Numerics;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.Entities.SAMP;

internal class ServerService : IServerService
{
    private readonly IActorsComponent _actors;
    private readonly IPlayerPool _players;
    private readonly IConfig _config;
    private readonly ICore _core;
    private readonly IVehiclesComponent _vehicles;
    private readonly IClassesComponent _classes;

    public ServerService(OpenMp openMp)
    {
        _actors = openMp.Components.QueryComponent<IActorsComponent>();
        _config = openMp.Core.GetConfig();
        _players = openMp.Core.GetPlayers();
        _vehicles = openMp.Components.QueryComponent<IVehiclesComponent>();
        _classes = openMp.Components.QueryComponent<IClassesComponent>();
        _core = openMp.Core;
    }

    public int ActorPoolSize
    {
        get
        {
            var max = -1;

            foreach (var actor in _actors.AsPool())
            {
                var id = actor.GetID();
                if (id > max)
                {
                    max = id;
                }
            }

            return max;
        }
    }

    public int MaxPlayers => _config.GetInt("max_players");

    public string NetworkStats => throw new NotImplementedException();

    public int PlayerPoolSize
    {
        get
        {
            var max = -1;

            foreach (var player in _players.Entries())
            {
                var id = player.GetID();
                if (id > max)
                {
                    max = id;
                }
            }

            return max;
        }
    }

    public int TickCount => (int)_core.GetTickCount();
    public int TickRate => (int)_core.TickRate();

    public int VehiclePoolSize
    {
        get
        {
            var max = -1;

            foreach (var vehicle in _vehicles.AsPool())
            {
                var id = vehicle.GetID();
                if (id > max)
                {
                    max = id;
                }
            }

            return max;
        }
    }

    // TODO: convert classes to components

    public int AddPlayerClass(int teamId, int modelId, Vector3 spawnPosition, float angle, Weapon weapon1 = Weapon.None, int weapon1Ammo = 0, Weapon weapon2 = Weapon.None,
        int weapon2Ammo = 0, Weapon weapon3 = Weapon.None, int weapon3Ammo = 0)
    {
        var slots = new WeaponSlotData[WeaponSlots.MAX_WEAPON_SLOTS];
        
        slots[0] = new WeaponSlotData((byte)weapon1, weapon1Ammo);
        slots[1] = new WeaponSlotData((byte)weapon2, weapon2Ammo);
        slots[2] = new WeaponSlotData((byte)weapon3, weapon3Ammo);

        var weapons = new WeaponSlots(slots);

        var @class = _classes.Create(modelId, teamId, spawnPosition, angle, ref weapons);

        return @class.GetID();
    }

    public int AddPlayerClass(int modelId, Vector3 spawnPosition, float angle, Weapon weapon1 = Weapon.None, int weapon1Ammo = 0, Weapon weapon2 = Weapon.None, int weapon2Ammo = 0,
        Weapon weapon3 = Weapon.None, int weapon3Ammo = 0)
    {
        return AddPlayerClass(OpenMpConstants.TEAM_NONE, modelId, spawnPosition, angle, weapon1, weapon1Ammo, weapon2, weapon2Ammo, weapon3, weapon3Ammo);
    }

    public void BlockIpAddress(string ipAddress, TimeSpan time = default)
    {
        throw new NotImplementedException();
    }

    public void ConnectNpc(string name, string script)
    {
        _core.ConnectBot(name, script);
    }

    public void DisableInteriorEnterExits()
    {
        ref var fld = ref _core.GetConfig().GetBool("game.use_entry_exit_markers");
        fld = false;
    }

    public void EnableStuntBonus(bool enable)
    {
        _core.UseStuntBonuses(enable);
    }

    public void EnableVehicleFriendlyFire()
    {
        ref var fld = ref _core.GetConfig().GetBool("game.use_vehicle_friendly_fire");
        fld = false;
    }

    public void GameModeExit()
    {
        throw new NotImplementedException();
    }

    public bool GetConsoleVarAsBool(string variableName)
    {
        throw new NotImplementedException();
    }

    public int GetConsoleVarAsInt(string variableName)
    {
        throw new NotImplementedException();
    }

    public string GetConsoleVarAsString(string variableName)
    {
        throw new NotImplementedException();
    }

    public void LimitGlobalChatRadius(float chatRadius)
    {
        throw new NotImplementedException();
    }

    public void LimitPlayerMarkerRadius(float markerRadius)
    {
        throw new NotImplementedException();
    }

    public void ManualVehicleEngineAndLights()
    {
        throw new NotImplementedException();
    }

    public void SendRconCommand(string command)
    {
        throw new NotImplementedException();
    }

    public void SetGameModeText(string text)
    {
        throw new NotImplementedException();
    }

    public void SetNameTagDrawDistance(float distance = 70)
    {
        throw new NotImplementedException();
    }

    public void SetTeamCount(int count)
    {
        throw new NotImplementedException();
    }

    public void SetWorldTime(int hour)
    {
        throw new NotImplementedException();
    }

    public void ShowNameTags(bool show)
    {
        throw new NotImplementedException();
    }

    public void ShowPlayerMarkers(PlayerMarkersMode mode)
    {
        throw new NotImplementedException();
    }

    public void UnBlockIpAddress(string ipAddress)
    {
        throw new NotImplementedException();
    }

    public void UsePlayerPedAnims()
    {
        throw new NotImplementedException();
    }
}