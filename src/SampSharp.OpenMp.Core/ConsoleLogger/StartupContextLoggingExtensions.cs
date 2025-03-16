using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public static class StartupContextLoggingExtensions
{
    /// <summary>
    /// When called, sets the console output and error streams to log to the open.mp logger.
    /// </summary>
    /// <param name="context">The startup context.</param>
    /// <returns>The startup context.</returns>
    public static IStartupContext UseOpenMpLogger(this IStartupContext context)
    {
        Console.SetOut(new LoggerTextWriter((ILogger)context.Core, LogLevel.Message));
        Console.SetError(new LoggerTextWriter((ILogger)context.Core, LogLevel.Error));

        return context;
    }

}