using Microsoft.Extensions.DependencyInjection;
using SampSharp.OpenMp.Core;

namespace SampSharp.Entities;

public interface IEcsStartup : IStartup
{
    void Configure(IServiceCollection services);
    void Configure(IEcsBuilder builder);
}