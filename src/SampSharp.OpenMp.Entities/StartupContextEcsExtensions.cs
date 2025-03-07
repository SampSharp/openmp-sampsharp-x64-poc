using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public static class StartupContextEcsExtensions
{
    public static StartupContext UseEntities(this StartupContext context, Action<EcsConfiguration>? configure = null)
    {
        if (context.Configurator is not IEcsStartup)
        {
            throw new InvalidOperationException("The startup type does not implement the 'IEcsStartup' interface.");
        }

        var config = new EcsConfiguration();

        configure?.Invoke(config);

        var manager = new EcsManager(config);
        context.Core.AddExtension(manager);

        context.Initialized += OnContextInitialized; 
        context.Cleanup += OnContextCleanup;

        return context;
    }
    
    private static void OnContextInitialized(object? sender, EventArgs e)
    {
        var context = (StartupContext)sender!;
        var manager = context.Core.GetExtension<EcsManager>();

        manager.Run(context);
    }

    private static void OnContextCleanup(object? sender, EventArgs e)
    {
        var context = (StartupContext)sender!;
        var manager = context.Core.GetExtension<EcsManager>();

        context.Core.RemoveExtension(manager);
    }
}