using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public class StartupContext
{
    private ExceptionHandler _unhandledExceptionHandler;

    public StartupContext(SampSharpInitParams init)
    {
        Core = init.Core;
        ComponentList = init.ComponentList;
        Info = init.Info;
        _unhandledExceptionHandler = (context, ex) =>
        {
            Core.LogLine(LogLevel.Error, $"Unhandled exception during {context}:");
            Core.LogLine(LogLevel.Error, ex.ToString());
        };
        SampSharpExceptionHandler.SetExceptionHandler(_unhandledExceptionHandler);
    }

    public ICore Core { get; }
    public IComponentList ComponentList { get; }
    public SampSharpInfo Info { get; }

    public ExceptionHandler UnhandledExceptionHandler
    {
        get => _unhandledExceptionHandler;
        set
        {
            _unhandledExceptionHandler = value;
            SampSharpExceptionHandler.SetExceptionHandler(value);
        }
    }

    public Action? Cleanup { get; set; }

    public static void MainInfoProvider()
    {
        LaunchInstructions.Write();
    }
}