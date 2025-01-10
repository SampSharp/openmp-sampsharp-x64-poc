using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

/// <summary>
/// Information about a marshaller mode as provided by the CustomMarshallerAttribute on the marshaller entry point.
/// </summary>
/// <param name="ManagedType">The managed type to marshal.</param>
/// <param name="MarshalMode">The marshalling mode this applies to.</param>
/// <param name="MarshallerType">The type used for marshalling.</param>
public record CustomMarshallerInfo(ITypeSymbol ManagedType, MarshalMode MarshalMode, ITypeSymbol MarshallerType);