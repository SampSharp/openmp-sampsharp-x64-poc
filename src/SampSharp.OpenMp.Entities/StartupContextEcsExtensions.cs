using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public static class StartupContextEcsExtensions
{
    public static IStartupContext UseEntities(this IStartupContext context, Action<EcsConfiguration>? configure = null)
    {
        if (context.Configurator is not IEcsStartup)
        {
            throw new InvalidOperationException("The startup type does not implement the 'IEcsStartup' interface.");
        }

        if (context.Core.TryGetExtension<EcsManager>() != null)
        {
            throw new InvalidOperationException("ECS has already been configured.");
        }

        context.UseSynchronizationContext();
        
        context.Initialized += OnContextInitialized; 
        context.Cleanup += OnContextCleanup;

        context.Core.AddExtension(
            new EcsManager(
                EcsConfiguration.Create(configure)));

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
        context.Core.GetExtension<EcsManager>()?.Dispose();
    }
}