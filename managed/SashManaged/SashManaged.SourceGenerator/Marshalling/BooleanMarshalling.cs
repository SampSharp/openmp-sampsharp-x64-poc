using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SashManaged.SourceGenerator.Marshalling;

public class BooleanMarshalling : Marshaller
{
    public static BooleanMarshalling Instance { get; } = new();

    public override bool RequiresMarshalling => false;
    public override bool RequiresUnsafe => false;

    public override TypeSyntax GetExternalType(ITypeSymbol typeSymbol)
    {
        return ParseTypeName($"global::{Constants.BlittableBooleanFQN}");
    }
}