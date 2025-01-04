using System.Numerics;
using System.Runtime.InteropServices;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public class Interop : 
    // ICoreEventHandler,
    IConsoleEventHandler
{
    private static ICore _core;
    private static IVehiclesComponent _vehicles;
    private static IPlayerPool _players;

    public void OnTick(Microseconds micros, TimePoint now)
    {
    }
    

    [UnmanagedCallersOnly]
    public static void OnInit(ICore core, IComponentList componentList)
    {
        _core = core;

        Console.WriteLine("OnInit from managed c# code!");
        Console.WriteLine($"Network bit stream version: {core.GetNetworkBitStreamVersion()}");

        Console.WriteLine($"core version: {core.GetVersion()}");

        var cfg = core.GetConfig();

        // test config
        var name = cfg.GetString("name");
        var announce = cfg.GetBool("announce");
        var lanMode = cfg.GetBool("network.use_lan_mode");
        var inputFilter = cfg.GetBool("chat_input_filter");
        Console.WriteLine($"name: {name} announce: {announce} use_lan_mode: {lanMode} chat_input_filter: {inputFilter}");

        // test bans

        cfg.AddBan(new BanEntry("1.2.3.5", DateTimeOffset.UtcNow, "name", "reason"));
        cfg.AddBan(new BanEntry("1.2.3.6", DateTimeOffset.UtcNow, "name", "reason2"));
        cfg.WriteBans();

        Console.WriteLine("written ban");

        for (nint i = 0; i < cfg.GetBansCount().Value; i++)
        {
            var b = cfg.GetBan(new Size(i));
            Console.WriteLine($"ban: {b.Name} {b.Address} {b.Reason} {b.Time}");
        }

        _vehicles = componentList.QueryComponent<IVehiclesComponent>();

        // test handlers
        var players = core.GetPlayers();
        _players = players;
        var handler = new Interop();

        var dispatcher = core.GetEventDispatcher();
        Console.WriteLine($"COUNT before:::::::::::::::::::::::::::: {dispatcher.Count().Value}");
        // dispatcher.AddEventHandler(handler);
        Console.WriteLine($"COUNT after:::::::::::::::::::::::::::: {dispatcher.Count().Value}");

        componentList.QueryComponent<IConsoleComponent>().GetEventDispatcher().AddEventHandler(handler);

        var v = _vehicles.Create(false, 401, new Vector3(5, 0, 10));
        v.GetColour(out var vcol);
        Console.WriteLine($"vehicle color: <1: {vcol.First}, 2: {vcol.Second}>");
    }

    // need an entry point to build runtime config for this application
    public static void Main()
    {
        /*nop*/
    }
    
    public bool OnConsoleText(string command, string parameters, ref ConsoleCommandSenderData sender)
    {
        if (command == "banana")
        {
            Console.WriteLine($"BANANA!!! {parameters}");
            return true;
        }

        Console.WriteLine($"cmd: {command}; params: {parameters}");
        return false;
    }

    public bool OnConsoleText(StringView command, StringView parameters, ref ConsoleCommandSenderData sender)
    {
        if (command.ToString() == "banana")
        {
            Console.WriteLine($"BANANA!!! {parameters}");
            return true;
        }

        Console.WriteLine($"cmd (SV): {command}; params: {parameters}");
        return false;
    }

    public void OnRconLoginAttempt(IPlayer player, StringView password, bool success)
    {
        Console.WriteLine($"login attempt by player {player.Handle} w/pw {password}; {success}");
    }

    public void OnConsoleCommandListRequest(FlatHashSetStringView commands)
    {
        commands.Emplace("banana");
    }
}