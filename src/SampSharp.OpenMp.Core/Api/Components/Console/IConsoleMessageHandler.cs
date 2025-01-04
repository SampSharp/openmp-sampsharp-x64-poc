namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi]
public readonly partial struct IConsoleMessageHandler
{
    public partial void HandleConsoleMessage(string message);
}