using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator.Marshalling;

public record MarshallerModeInfo(ITypeSymbol ManagedType, MarshallerModeValue Mode, ITypeSymbol MarshallerType);