using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public sealed class StartupContext : IStartupContext
{
    private IStartup? _configurator;
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
    public IStartup Configurator => _configurator ?? throw new InvalidOperationException("The configurator has not been set.");
    public ExceptionHandler UnhandledExceptionHandler
    {
        get => _unhandledExceptionHandler;
        set
        {
            _unhandledExceptionHandler = value;
            SampSharpExceptionHandler.SetExceptionHandler(value);
        }
    }

    public event EventHandler? Cleanup;
    public event EventHandler? Initialized;

    /// <summary>
    /// Internal method. Do not invoke manually.
    /// </summary>
    public void InitializeUsing(IStartup configurator)
    {
        _configurator = configurator;

        configurator.Initialize(this);
        Initialized?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Internal method. Do not invoke manually.
    /// </summary>
    public void InvokeCleanup()
    {
        Cleanup?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Internal method. Do not invoke manually.
    /// </summary>
    public static void MainInfoProvider()
    {
        LaunchInstructions.Write();
    }
}