using System;
using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator;

public record WellKnownMarshallerTypes((Func<ITypeSymbol, bool> matcher, INamedTypeSymbol marshaller)[] Marshallers);