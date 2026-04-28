namespace SampSharp.Entities;

/// <summary>Provides functionality for configuring the SampSharp EntityComponentSystem.</summary>
public interface IEcsApplicationBuilder
{
    /// <summary>Gets the service provider.</summary>
    IServiceProvider Services { get; }
}