using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace SampSharp.Entities.Logging;

public static class OmpLoggerProviderExtensions
{
    public static void AddOpenMp(this ILoggingBuilder builder, LogLevel minLogLevel = LogLevel.Trace)
    {
        builder.Services.TryAddSingleton<ILoggerProvider>(sp => 
            new OmpLoggerProvider((SampSharp.OpenMp.Core.Api.ILogger)sp.GetRequiredService<OpenMp>().Core, minLogLevel));
    }
}

internal class OmpLoggerProvider(SampSharp.OpenMp.Core.Api.ILogger innerLogger, LogLevel minLogLevel) : ILoggerProvider
{
    private readonly ObjectPool<StringBuilder> _stringBuilders = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
    private readonly ConcurrentDictionary<string, ILogger> _loggers = [];
    
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, CreateNewLogger);
    }

    private ILogger CreateNewLogger(string name)
    {
        return new OmpLogger(innerLogger, minLogLevel, name, _stringBuilders);
    }

    public void Dispose()
    {
        //
    }
}