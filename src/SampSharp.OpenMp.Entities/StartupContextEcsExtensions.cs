using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public static class StartupContextEcsExtensions
{
    public static StartupContext UseEntities(this StartupContext context)
    {
        if (context.Configurator is not IEcsStartup)
        {
            throw new InvalidOperationException("The startup type does not implement the 'IEcsStartup' interface.");
        }

        var manager = new EcsManager();
        context.Core.AddExtension(manager);
        context.Cleanup += (sender, args) => context.Core.RemoveExtension(manager);

        context.Initialized += OnContextInitialized; 
        return context;
    }

    private static void OnContextInitialized(object? sender, EventArgs e)
    {
        var context = (StartupContext)sender!;
        var manager = context.Core.GetExtension<EcsManager>();

        manager.Initialize(context);
    }
}