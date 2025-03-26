namespace SampSharp.OpenMp.Core.Api;

/// <summary>
/// This type represents a pointer to an unmanaged open.mp <see cref="IConsoleComponent"/> interface.
/// </summary>
[OpenMpApi(typeof(IComponent))]
public readonly partial struct IConsoleComponent
{
    public static UID ComponentId => new(0xbfa24e49d0c95ee4);

    public partial IEventDispatcher<IConsoleEventHandler> GetEventDispatcher();

    public partial void Send(string command, ref ConsoleCommandSenderData sender);
    public partial void SendMessage(ref ConsoleCommandSenderData recipient, string message);
}