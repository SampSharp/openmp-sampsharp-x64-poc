using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using SashManaged.Chrono;
using SashManaged.OpenMp;

namespace SashManaged;

public class Interop : IPlayerConnectEventHandler, ICoreEventHandler, IPlayerSpawnEventHandler, IPlayerShotEventHandler
{
    private static ICore _core;
    private static IVehiclesComponent _vehicles;

    public void OnTick(Microseconds micros, TimePoint now)
    {
        //Console.WriteLine($"micros: {micros.AsTimeSpan()}, now: {now}");
    }

    public void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port)
    {
    }

    public unsafe void OnPlayerConnect(IPlayer player)
    {
        var view = player.GetName();

        Console.WriteLine($"Player {view} connected!");


        var col = new Colour(255, 255, 255, 255);

        var bytes = Encoding.UTF8.GetBytes($"Welcome, {view}!");

        fixed (byte* pin = bytes)
        {
            player.SendClientMessage(ref col, new StringView(pin, bytes.Length));
        }
    }

    public void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
    }

    public void OnPlayerClientInit(IPlayer player)
    {
    }

    public unsafe bool OnPlayerShotMissed(IPlayer player, PlayerBulletDataPtr bulletData)
    {
        var col = new Colour(255, 255, 255, 255);

        var msg =
            $"Your shot missed @ hit {bulletData.Value.hitPos}, from {bulletData.Value.origin}, offset {bulletData.Value.offset}, weapon {bulletData.Value.weapon} type {bulletData.Value.hitType} id {bulletData.Value.hitID}";

        Console.WriteLine(msg);
        var bytes = Encoding.UTF8.GetBytes(msg);

        fixed (byte* pin = bytes)
        {
            player.SendClientMessage(ref col, new StringView(pin, bytes.Length));
        }

        return true;
    }

    public bool OnPlayerShotPlayer(IPlayer player, IPlayer target, PlayerBulletDataPtr bulletData)
    {
        return true;
    }

    public unsafe bool OnPlayerShotVehicle(IPlayer player, IVehicle target, PlayerBulletDataPtr bulletData)
    {
        var col = new Colour(255, 255, 255, 255);

        var msg =
            $"Your shot vehicle @ hit {bulletData.Value.hitPos}, from {bulletData.Value.origin}, offset {bulletData.Value.offset}, weapon {bulletData.Value.weapon} type {bulletData.Value.hitType} id {bulletData.Value.hitID}";
        var bytes = Encoding.UTF8.GetBytes(msg.Substring(0, 143));

        Console.WriteLine(msg);
        fixed (byte* pin = bytes)
        {
            player.SendClientMessage(ref col, new StringView(pin, bytes.Length));
        }

        return true;
    }

    public bool OnPlayerShotObject(IPlayer player, IObject target, PlayerBulletDataPtr bulletData)
    {
        return true;
    }

    public bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, PlayerBulletDataPtr bulletData)
    {
        return true;
    }

    public bool OnPlayerRequestSpawn(IPlayer player)
    {
        return true;
    }

    public void OnPlayerSpawn(IPlayer player)
    {
        var pos = new Vector3(0, 0, 10);
        player.SetPosition(pos);

        player.GiveWeapon(new WeaponSlotData((int)PlayerWeapon.AK47, 200));
        _vehicles.Create(false, 401, new Vector3(5, 0, 10));
    }

    [UnmanagedCallersOnly]
    public static void OnInit(ICore core, IComponentList componentList)
    {
        _core = core;
        Console.WriteLine("OnInit from managed c# code!");
        Console.WriteLine($"Network bit stream version: {core.GetNetworkBitStreamVersion()}");

        Console.WriteLine($"core version: {core.GetVersion()}");

        core.SetData(SettableCoreDataType.ServerName, "Hello from .NET code!!!"u8);

        var cfg = core.GetConfig();

        // test config
        var nameInConfig = cfg.GetString("name"u8);
        Console.WriteLine($"Name in config: {nameInConfig}");
        var announce = cfg.GetBool("announce"u8);
        var use_lan_mode = cfg.GetBool("network.use_lan_mode"u8);
        Console.WriteLine($"announce: {announce} use_lan_mode: {use_lan_mode}");

        // test bans
        var ban = new BanEntry(new HybridString46("1.2.3.4"), 1234, new HybridString25("name"), new HybridString32("reason"));

        cfg.AddBan(ban);
        cfg.WriteBans();
        _vehicles = componentList.QueryComponent<IVehiclesComponent>();

        // test handlers
        var players = core.GetPlayers();
        var handler = new Interop();

        players.GetPlayerSpawnDispatcher().AddEventHandler(handler);
        players.GetPlayerConnectDispatcher().AddEventHandler(handler);
        players.GetPlayerShotDispatcher().AddEventHandler(handler);
        core.GetEventDispatcher().AddEventHandler(handler);
    }

    // need an entry point to build runtime config for this application
    public static void Main()
    {
        /*nop*/
    }
}