using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using SashManaged.OpenMp;
using IComponentList = SashManaged.OpenMp.IComponentList;

namespace SashManaged;


public partial class Testing
{
    //[System.Runtime.InteropServices.DllImport("SampSharp", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
    
    //[LibraryImport("SampSharp")]
    //public static partial IVehicle IVehiclesComponent_create(IVehiclesComponent ptr, BlittableBoolean isStatic, int modelID, Vector3 position, float Z, int colour1, int colour2, int respawnDelay, BlittableBoolean addSiren);
    
    [LibraryImport("SampSharp")]
    public static partial void ICore_setData(ICore ptr, SashManaged.OpenMp.SettableCoreDataType type, [System.Runtime.InteropServices.Marshalling.MarshalUsing(typeof(StringViewMarshaller))] string data);
}

[OpenMpApi2]
public partial struct TestV2
{
    public partial ref int Testing123(ref int a, bool b, string c);
}

public class Interop : IPlayerConnectEventHandler, ICoreEventHandler, IPlayerSpawnEventHandler, IPlayerShotEventHandler, IPlayerPoolEventHandler, IConsoleEventHandler
{
    private static ICore _core;
    private static IVehiclesComponent _vehicles;

    #region Handlers

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

        Console.WriteLine("iter players...");
        foreach (var player1 in _players.Players())
        {
            Console.WriteLine($"Name: {player1.GetName()}");
        }
        Console.WriteLine("...iter players");
    }

    public void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
    }

    public void OnPlayerClientInit(IPlayer player)
    {
    }

    public unsafe bool OnPlayerShotMissed(IPlayer player, ref PlayerBulletData bulletData)
    {
        var col = new Colour(255, 255, 255, 255);

        var msg =
            $"Your shot missed @ hit {bulletData.hitPos}, from {bulletData.origin}, offset {bulletData.offset}, weapon {bulletData.weapon} type {bulletData.hitType} id {bulletData.hitID}";

        Console.WriteLine(msg);
        var bytes = Encoding.UTF8.GetBytes(msg);

        fixed (byte* pin = bytes)
        {
            player.SendClientMessage(ref col, new StringView(pin, bytes.Length));
        }

        return true;
    }

    public bool OnPlayerShotPlayer(IPlayer player, IPlayer target, ref PlayerBulletData bulletData)
    {
        return true;
    }

    public unsafe bool OnPlayerShotVehicle(IPlayer player, IVehicle target, ref PlayerBulletData bulletData)
    {
        var col = new Colour(255, 255, 255, 255);

        var msg =
            $"Your shot vehicle @ hit {bulletData.hitPos}, from {bulletData.origin}, offset {bulletData.offset}, weapon {bulletData.weapon} type {bulletData.hitType} id {bulletData.hitID}";
        var bytes = Encoding.UTF8.GetBytes(msg.Substring(0, 143));

        Console.WriteLine(msg);
        fixed (byte* pin = bytes)
        {
            player.SendClientMessage(ref col, new StringView(pin, bytes.Length));
        }

        return true;
    }

    public bool OnPlayerShotObject(IPlayer player, IObject target, ref PlayerBulletData bulletData)
    {
        return true;
    }

    public bool OnPlayerShotPlayerObject(IPlayer player, IPlayerObject target, ref PlayerBulletData bulletData)
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

    public void OnPoolEntryCreated(IPlayer entry)
    {
        Console.WriteLine($"Pool entry created for player {entry.GetName()}");
    }

    public void OnPoolEntryDestroyed(IPlayer entry)
    {
        Console.WriteLine($"Pool entry removed for player {entry.GetName()}");
    }

    public bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender)
    {
        Console.WriteLine($"on console text {command} || {parameters}");
        return false;
    }

    public void OnRconLoginAttempt(IPlayer player, StringView password, bool success)
    {
        Console.WriteLine("rcon login attempt");
    }

    public void OnConsoleCommandListRequest(FlatHashSetStringView commands)
    {
        Console.WriteLine("command list request");
        commands.Emplace("banana"u8);
        foreach (var txt in commands)
        {
            Console.WriteLine("SASH> " + txt);
        }
    }

    #endregion

    private static IPlayerPool _players;

    [UnmanagedCallersOnly]
    public static void OnInit(ICore core, IComponentList componentList)
    {
        _core = core;
        Console.WriteLine("OnInit from managed c# code!");
        Console.WriteLine($"Network bit stream version: {core.GetNetworkBitStreamVersion()}");

        Console.WriteLine($"core version: {core.GetVersion()}");

        Testing.ICore_setData(core, SettableCoreDataType.ServerName, "This is getting marshalled!!!");

        //core.SetData(SettableCoreDataType.ServerName, "Hello from .NET code!!!"u8);

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

        var alias = cfg.GetNameFromAlias("minconnectiontime"u8);

        Console.WriteLine($"alias: {alias.First} {alias.Second}");
        _vehicles = componentList.QueryComponent<IVehiclesComponent>();

        // test handlers
        var players = core.GetPlayers();
        _players = players;
        var handler = new Interop();

        players.GetPlayerSpawnDispatcher().AddEventHandler(handler);
        players.GetPlayerConnectDispatcher().AddEventHandler(handler);
        players.GetPlayerShotDispatcher().AddEventHandler(handler);
        core.GetEventDispatcher().AddEventHandler(handler);

        componentList.QueryComponent<IConsoleComponent>().GetEventDispatcher().AddEventHandler(handler);
        players.GetPoolEventDispatcher().AddEventHandler(handler);
        Console.WriteLine("iter players...");
        foreach (var player1 in _players.Players())
        {
            Console.WriteLine($"Name: {player1.GetName()}");
        }
        Console.WriteLine("...iter players");

        var v = _vehicles.Create(false, 401, new Vector3(5, 0, 10));
        v.SetColour(1, 2);
        var (vcol1, vcol2) = v.GetColour();
        Console.WriteLine($"vehicle color: <1: {vcol1}, 2: {vcol2}>");
    }

    // need an entry point to build runtime config for this application
    public static void Main()
    {
        /*nop*/
    }
}