using System.Runtime.InteropServices;
using System.Text;
using SashManaged.OpenMp;

namespace SashManaged;

public class Interop : IPlayerConnectEventHandler, ICoreEventHandler
{
    public void OnTick(Microseconds micros, TimePoint now)
    {
        //Console.WriteLine($"micros: {micros.AsTimeSpan()}, now: {now}");
    }

    public void OnIncomingConnection(IPlayer player, StringView ipAddress, ushort port)
    {
    }

    public void OnPlayerConnect(IPlayer player)
    {
        var view = player.GetName();
        
        Console.WriteLine($"Player {view.ToString()} connected!");
    }

    public void OnPlayerDisconnect(IPlayer player, PeerDisconnectReason reason)
    {
    }

    public void OnPlayerClientInit(IPlayer player)
    {
    }
    

    [UnmanagedCallersOnly]
    public static unsafe void OnInit(ICore core)
    {
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

        // test handlers
        var players = core.GetPlayers();

        var handler = new Interop();

        players.GetPlayerConnectDispatcher().AddEventHandler(handler);
        core.GetEventDispatcher().AddEventHandler(handler);
    }
    
    // need an entry point to build runtime config for this application
    public static void Main(){/*nop*/}
}