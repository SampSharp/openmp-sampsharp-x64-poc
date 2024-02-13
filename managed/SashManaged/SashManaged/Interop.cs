using System.Runtime.InteropServices;
using SashManaged.OpenMp;

namespace SashManaged;

public class Interop
{

    public class TestingPlayerConnectEventHandler : IPlayerConnectEventHandler
    {
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
    }

    [UnmanagedCallersOnly]
    public static void OnInit(ICore core)
    {
        // Testing123.TestClass.HelloFrom("me");
        Console.WriteLine("OnInit from managed c# code!");
        Console.WriteLine($"Network bit stream version: {core.GetNetworkBitStreamVersion()}");

        Console.WriteLine($"core version: {core.GetVersion()}");

        core.SetData(SettableCoreDataType.ServerName, new StringView("Hello from .NET code!!!"u8));

        // activate PlayerConnectEventHandler and set handler to test implementation
        var players = core.GetPlayers();
        var dispatcher = players.GetPlayerConnectDispatcher();

        var handler = new TestingPlayerConnectEventHandler();
        dispatcher.AddEventHandler(handler);
    }
    
    // need an entry point to build runtime config for this application
    public static void Main(){/*nop*/}
}

[AttributeUsage(AttributeTargets.Struct)]
public class OpenMpAttribute : Attribute
{

}