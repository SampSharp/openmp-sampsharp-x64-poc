using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public static class LoggingStartupContextExtensions
{
    public static StartupContext ForwardConsoleOutputToOpenMpLogger(this StartupContext context)
    {
        Console.SetOut(new LoggerTextWriter((ILogger)context.Core, LogLevel.Message));
        Console.SetError(new LoggerTextWriter((ILogger)context.Core, LogLevel.Error));

        return context;
    }

}