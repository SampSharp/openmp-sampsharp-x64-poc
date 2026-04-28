using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampSharp.Entities.SAMP.Commands;

/// <summary>Extensions to register the player-commands subsystem.</summary>
public static class EcsBuilderCommandsExtensions
{
    /// <summary>
    /// Registers <see cref="IPlayerCommandService"/> with the default
    /// <see cref="PlayerCommandService"/> implementation. Uses
    /// <see cref="ServiceCollectionDescriptorExtensions.TryAddSingleton(IServiceCollection, Type, Type)"/>
    /// so a custom <see cref="IPlayerCommandService"/> registered earlier wins.
    /// </summary>
    public static IServiceCollection AddPlayerCommands(this IServiceCollection services)
    {
        services.TryAddSingleton<IPlayerCommandService, PlayerCommandService>();
        return services;
    }

    /// <summary>
    /// Wires <see cref="PlayerCommandProcessingMiddleware"/> on
    /// <c>OnPlayerCommandText</c>: any chat input not claimed by an
    /// <c>[Event]</c> listener gets forwarded to <see cref="IPlayerCommandService"/>.
    /// </summary>
    public static IEcsBuilder UsePlayerCommands(this IEcsBuilder builder)
        => builder.UseMiddleware<PlayerCommandProcessingMiddleware>("OnPlayerCommandText");
}
