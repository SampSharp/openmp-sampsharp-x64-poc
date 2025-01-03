using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using SashManaged.OpenMp;

namespace SashManaged;

public class Interop : 
    // IPlayerConnectEventHandler,
    ICoreEventHandler//, 
    // IPlayerSpawnEventHandler, 
    // IPlayerShotEventHandler, 
    // IPlayerPoolEventHandler, 
    // IConsoleEventHandler
{
    private static ICore _core;
    private static IVehiclesComponent _vehicles;

    #region Handlers

    public void OnTick(Microseconds micros, TimePoint now)
    {
        // Console.WriteLine($"micros: {micros.AsTimeSpan()}, now: {now}");
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
        commands.Emplace("banana");
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

        var cfg = core.GetConfig();

        // test config
        var nameInConfig = cfg.GetString("name");
        Console.WriteLine($"Name in config: {nameInConfig}");
        var announce = cfg.GetBool("announce");
        var use_lan_mode = cfg.GetBool("network.use_lan_mode");
        var chat_input_filter = cfg.GetBool("chat_input_filter");
        Console.WriteLine($"announce: {announce} use_lan_mode: {use_lan_mode} chat_input_filter: {chat_input_filter}");

        // test bans

        var ban = new BanEntry("1.2.3.5", DateTimeOffset.UtcNow, "name", "reason");

        cfg.AddBan(ban);
        cfg.WriteBans();

        Console.WriteLine("written ban");

        for(nint i=0;i<cfg.GetBansCount().Value;i++)
        {
            var b = cfg.GetBan(new Size(i));
            Console.WriteLine($"ban: {b.Name} {b.Address} {b.Reason} {b.Time}");
        }

        // var alias = cfg.GetNameFromAlias("minconnectiontime"u8);

        // Console.WriteLine($"alias: {alias.First} {alias.Second}");
        _vehicles = componentList.QueryComponent<IVehiclesComponent>();

        // test handlers
        var players = core.GetPlayers();
        _players = players;
        var handler = new Interop();

        // players.GetPlayerSpawnDispatcher().AddEventHandler(handler);
        // players.GetPlayerConnectDispatcher().AddEventHandler(handler);
        // players.GetPlayerShotDispatcher().AddEventHandler(handler);

        var dispatcher = core.GetEventDispatcher();
        Console.WriteLine($"COUNT before:::::::::::::::::::::::::::: {dispatcher.Count().Value}");
        dispatcher.AddEventHandler(handler);
        Console.WriteLine($"COUNT after:::::::::::::::::::::::::::: {dispatcher.Count().Value}");

        // componentList.QueryComponent<IConsoleComponent>().GetEventDispatcher().AddEventHandler(handler);
        // players.GetPoolEventDispatcher().AddEventHandler(handler);
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