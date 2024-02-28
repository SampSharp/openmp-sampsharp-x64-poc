using System;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator;

public record WellKnownMarshallerTypes(params (Func<ITypeSymbol, bool> matcher, INamedTypeSymbol marshaller)[] Marshallers);