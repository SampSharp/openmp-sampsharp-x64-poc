namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IComponent))]
public readonly partial struct IConsoleComponent
{
    public static UID ComponentId => new(0xbfa24e49d0c95ee4);

    public partial IEventDispatcher<IConsoleEventHandler> GetEventDispatcher();

    public partial void Send(StringView command, ref ConsoleCommandSenderData sender);
    public partial void SendMessage(ref ConsoleCommandSenderData recipient, StringView message);
}