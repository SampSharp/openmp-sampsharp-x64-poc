using System;
using Microsoft.CodeAnalysis;

namespace SampSharp.SourceGenerator.Marshalling;

public record WellKnownMarshallerTypes(params (Func<ITypeSymbol, bool> matcher, INamedTypeSymbol? marshaller)[] Marshallers);