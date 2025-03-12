namespace SampSharp.Entities;

/// <summary>Provides the functionality for a registry of system types.</summary>
public interface ISystemRegistry
{
    /// <summary>Gets all types of systems of the specified <paramref name="type" />.</summary>
    /// <param name="type">The type of the systems to get.</param>
    /// <returns>An array of the systems of the specified type.</returns>
    ReadOnlyMemory<ISystem> Get(Type type);

    /// <summary>Gets all types of systems of the specified <typeparamref name="TSystem" />.</summary>
    /// <typeparam name="TSystem">The type of the systems to get.</typeparam>
    /// <returns>An array of the systems of the specified type.</returns>
    ReadOnlyMemory<ISystem> Get<TSystem>() where TSystem : ISystem;

    ReadOnlyMemory<Type> GetSystemTypes();

    void RegisterSystemsLoadedHandler(Action handler);
}