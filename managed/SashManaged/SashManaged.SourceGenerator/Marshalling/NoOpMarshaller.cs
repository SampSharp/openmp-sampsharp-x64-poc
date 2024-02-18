using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class NoOpMarshaller : Marshaller
{
    public static NoOpMarshaller Instance { get; } = new();
    public override bool RequiresMarshalling => false;
    public override bool RequiresUnsafe => false;

    public override TypeSyntax GetExternalType(ITypeSymbol typeSymbol)
    {
        return OpenMpApiCodeGenV2.ToTypeSyntax(typeSymbol);
    }
}