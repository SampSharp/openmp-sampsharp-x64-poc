using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

/// <summary>
/// Contains extension methods for <see cref="IStartupContext" /> to configure the ECS system.
/// </summary>
public static class StartupContextEcsExtensions
{
    /// <summary>
    /// Configures the ECS system for the SampSharp application.
    /// </summary>
    /// <param name="context">The startup context.</param>
    /// <param name="configure">An optional action to configure the ECS system.</param>
    /// <returns>The startup context.</returns>
    public static IEcsHostBuilder UseEntities(this IStartupContext context, Action<IEcsHostBuilder>? configure = null)
    {
        if (context.Core.TryGetExtension<EcsHost>() != null)
        {
            throw new InvalidOperationException("ECS has already been configured.");
        }

        var configuration = new EcsHostBuilder();
        configure?.Invoke(configuration);

        if (context.Configurator is IEcsStartup startup)
        {
            configuration.Configure(startup.Configure);
            configuration.ConfigureServices(startup.ConfigureServices);
        }

        new EcsHost(configuration).Bind(context);

        return configuration;
    }
}