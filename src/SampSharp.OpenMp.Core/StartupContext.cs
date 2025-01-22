using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;

public class StartupContext
{
    public StartupContext(SampSharpInitParams init)
    {
        Core = init.Core;
        ComponentList = init.ComponentList;
        Info = init.Info;
    }

    public ICore Core { get; }
    public IComponentList ComponentList { get; }
    public SampSharpInfo Info { get; }

    public Action? Cleanup { get; set; }

    public static void MainInfoProvider()
    {
        LaunchInstructions.Write();
    }
}