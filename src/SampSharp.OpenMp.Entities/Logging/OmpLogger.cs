using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using SampSharp.OpenMp.Core;
using OmpLogLevel = SampSharp.OpenMp.Core.Api.LogLevel;
namespace SampSharp.Entities.Logging;

internal class OmpLogger(SampSharp.OpenMp.Core.Api.ILogger inner, LogLevel minLogLevel, string name, ObjectPool<StringBuilder> objectPool) : ILogger
{
    private readonly Dictionary<OmpLogLevel, LoggerTextWriter> _writers = new()
    {
        [OmpLogLevel.Debug] = new LoggerTextWriter(inner, OmpLogLevel.Debug),
        [OmpLogLevel.Message] = new LoggerTextWriter(inner, OmpLogLevel.Message),
        [OmpLogLevel.Warning] = new LoggerTextWriter(inner, OmpLogLevel.Warning),
        [OmpLogLevel.Error] = new LoggerTextWriter(inner, OmpLogLevel.Error)
    };

public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var sb = objectPool.Get();

        try
        {
            if (eventId.Id != 0)
            {
                sb.Append($"[{eventId.Id,2}]`");
            }

            if (logLevel is LogLevel.Trace or LogLevel.Critical)
            {
                sb.Append($" [{logLevel}]");
            }

            sb.Append($"{name} - {formatter(state, exception)}");

            if (exception != null)
            {
                sb.AppendLine();
                sb.Append(exception.StackTrace);
            }

            _writers[Convert(logLevel)].WriteLine(sb.ToString());
        }
        finally
        {
            objectPool.Return(sb);
        }
    }

    private static OmpLogLevel Convert(LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => OmpLogLevel.Debug,
            LogLevel.Debug => OmpLogLevel.Debug,
            LogLevel.Warning => OmpLogLevel.Warning,
            LogLevel.Error => OmpLogLevel.Error,
            LogLevel.Critical => OmpLogLevel.Error,
            _ => OmpLogLevel.Message
        };
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= minLogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}