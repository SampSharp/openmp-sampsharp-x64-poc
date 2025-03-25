using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

/// <summary>
/// Contains extension methods for <see cref="IStartupContext"/> to configure the ECS system.
/// </summary>
public static class StartupContextEcsExtensions
{
    /// <summary>
    /// Configures the ECS system for the SampSharp application.
    /// </summary>
    /// <param name="context">The startup context.</param>
    /// <param name="configure">An optional action to configure the ECS system.</param>
    /// <returns>The startup context.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the startup type does not implement the <see cref="IEcsStartup"/> interface or if ECS has already been configured.</exception>
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