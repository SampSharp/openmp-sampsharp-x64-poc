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

        if (context.Core.TryGetExtension<EcsConfigurator>() != null)
        {
            throw new InvalidOperationException("ECS has already been configured.");
        }

        new EcsConfigurator(EcsConfiguration.Create(configure)).Bind(context);

        return context;
    }
}