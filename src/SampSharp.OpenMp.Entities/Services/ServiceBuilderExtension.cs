using Microsoft.Extensions.DependencyInjection;
using SampSharp.Entities.Containers;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

[Extension(0xa4a0403ac8b351dd)]
internal class ServiceBuilderExtension : Extension
{
    public Func<IServiceCollection, IServiceProvider> ServiceProviderFactory { get; set; } = services =>
    {
        // default factory
        var factory = new DefaultServiceContainerFactory();
        var builder = factory.CreateBuilder(services);
        return factory.CreateServiceProvider(builder);
    };
}