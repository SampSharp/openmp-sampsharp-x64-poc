namespace SampSharp.Entities;

internal class EcsApplicationBuilder(IServiceProvider services) : IEcsApplicationBuilder
{
    public IServiceProvider Services { get; } = services;
}