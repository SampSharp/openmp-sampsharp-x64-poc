using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public record MarshallerModeInfo(ITypeSymbol ManagedType, MarshallerModeValue Mode, ITypeSymbol MarshallerType);