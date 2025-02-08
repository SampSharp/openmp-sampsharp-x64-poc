using System.Numerics;
using SampSharp.OpenMp.Core;
using SampSharp.OpenMp.Core.Api;

namespace TestMode.OpenMp.Core;

public class Startup : IStartup,
    ICoreEventHandler,
    IConsoleEventHandler, 
    IPlayerPoolEventHandler
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

        Console.WriteLine("add extension");
        v.AddExtension(new Nickname("Brum"));

        Console.WriteLine("get extension");
        var nick = v.TryGetExtension<Nickname>();
        Console.WriteLine((nick?.ToString() ?? "null"));

        Console.WriteLine("remove extension");
        v.RemoveExtension(nick);

        Console.WriteLine("get extension");
        nick = v.TryGetExtension<Nickname>();
        Console.WriteLine((nick?.ToString() ?? "null"));

        
        var pool = context.Core.GetPlayers().GetPoolEventDispatcher();
        Console.WriteLine("count before " + pool.Count());
        pool.AddEventHandler(this);
        Console.WriteLine("count after " + pool.Count());

        var tds = context.ComponentList.QueryComponent<ITextDrawsComponent>();
        var txd = tds.Create(new Vector2(0, 0), 99);
        var txt = txd.GetText();
        Console.WriteLine($"textdraw text: '{txt ?? "<<null>>"}'");
        Console.WriteLine($"default plate: '{v.GetPlate() ?? "<<null>>"}'");

        Console.WriteLine("<write>");
        context.Core.PrintLine("Hello, World!");
        context.Core.LogLine(LogLevel.Error, "Hello, World!");
        Console.WriteLine("</write>");
        // try
        // {
        //     throw new Exception("awful");
        // }
        // catch(Exception e)
        // {
        //     SampSharpExceptionHandler.HandleException("test", e);
        // }
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

    public void OnPoolEntryCreated(IPlayer entry)
    {
    }

    public void OnPoolEntryDestroyed(IPlayer entry)
    {
    }
}


[Extension(0xBBDD9B761EB23037)]
public class Nickname : Extension
{
    public Nickname(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString()
    {
        return $"{{name: {Name}}}";
    }
}
