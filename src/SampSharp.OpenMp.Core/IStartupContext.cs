using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public interface IStartupContext
{
    IComponentList ComponentList { get; }
    IStartup Configurator { get; }
    ICore Core { get; }
    SampSharpInfo Info { get; }
    ExceptionHandler UnhandledExceptionHandler { get; set; }

    event EventHandler? Cleanup;
    event EventHandler? Initialized;
}