using Microsoft.CodeAnalysis;

namespace SashManaged.SourceGenerator.Marshalling.Stateful;

public abstract class StatefulMarshallerShape(string nativeTypeName, string marshallerTypeName) : MarshallerShape(nativeTypeName, marshallerTypeName)
{
    protected static string GetMarshallerVar(IParameterSymbol parameterSymbol)
    {
        return $"__{(parameterSymbol?.Name ?? "retVal")}_native_marshaller";
    }
}