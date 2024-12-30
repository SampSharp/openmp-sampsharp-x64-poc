using System;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator.Marshalling;

public record WellKnownMarshallerTypes(params (Func<ITypeSymbol, bool> matcher, INamedTypeSymbol? marshaller)[] Marshallers);