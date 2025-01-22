using System.Numerics;
using SampSharp.OpenMp.Core;
using SampSharp.OpenMp.Core.Api;

namespace TestMode.OpenMp.Core;

public class Startup : IStartup,
    ICoreEventHandler,
    IConsoleEventHandler
{
    private IVehiclesComponent _vehicles;

    public void Initialize(StartupContext context)
    {
        Console.WriteLine("OnInit from managed c# code!");

        Console.WriteLine($"Component version: {context.Info.Version}");
        Console.WriteLine($"size {context.Info.Size.Value} api {context.Info.ApiVersion}");
        Console.WriteLine($"Network bit stream version: {context.Core.GetNetworkBitStreamVersion()}");

        Console.WriteLine($"core version: {context.Core.GetVersion()}");

        var cfg = context.Core.GetConfig();

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

        _vehicles = context.ComponentList.QueryComponent<IVehiclesComponent>();

        // test handlers
        var dispatcher = context.Core.GetEventDispatcher();
        Console.WriteLine($"COUNT before:::::::::::::::::::::::::::: {dispatcher.Count()}");
        dispatcher.AddEventHandler(this);
        Console.WriteLine($"COUNT after:::::::::::::::::::::::::::: {dispatcher.Count()}");

        context.ComponentList.QueryComponent<IConsoleComponent>().GetEventDispatcher().AddEventHandler(this);

        var v = _vehicles.Create(false, 401, new Vector3(5, 0, 10));
        v.GetColour(out var vcol);
        Console.WriteLine($"vehicle color: <1: {vcol.First}, 2: {vcol.Second}>");
    }

    public void OnTick(Microseconds micros, TimePoint now)
    {
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

    public void OnRconLoginAttempt(IPlayer player, string password, bool success)
    {
        Console.WriteLine($"login attempt by player {player.GetName()}({player.Handle:X}) w/pw {password}; {success}");
    }

    public void OnConsoleCommandListRequest(FlatHashSetStringView commands)
    {
        commands.Emplace("banana");
    }
}