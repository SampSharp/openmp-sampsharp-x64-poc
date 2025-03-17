﻿namespace SampSharp.Entities;

/// <summary>Provides functionality for configuring the SampSharp EntityComponentSystem.</summary>
public interface IEcsBuilder
{
    /// <summary>Gets the service provider.</summary>
    IServiceProvider Services { get; }
}