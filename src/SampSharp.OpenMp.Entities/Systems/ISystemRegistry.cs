namespace SampSharp.Entities;

/// <summary>Provides the functionality for a registry of system types.</summary>
public interface ISystemRegistry
{
    /// <summary>Sets the systems available in this registry and locks further changes to the registry.</summary>
    /// <param name="types">The types of systems which should be available in the registry.</param>
    void Configure(Type[] types);

    /// <summary>Gets all types of systems of the specified <paramref name="type" />.</summary>
    /// <param name="type">The type of the systems to get.</param>
    /// <returns>An array of the systems of the specified type.</returns>
    ReadOnlyMemory<ISystem> Get(Type type);

    /// <summary>Gets all types of systems of the specified <typeparamref name="TSystem" />.</summary>
    /// <typeparam name="TSystem">The type of the systems to get.</typeparam>
    /// <returns>An array of the systems of the specified type.</returns>
    ReadOnlyMemory<ISystem> Get<TSystem>() where TSystem : ISystem;
}