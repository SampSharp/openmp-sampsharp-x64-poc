using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SampSharp.SourceGenerator.SyntaxFactories;

namespace SampSharp.SourceGenerator.Marshalling.V2.ShapeGenerators;

public class EmptyMarshalShapeGenerator : IMarshalShapeGenerator
{
    private EmptyMarshalShapeGenerator()
    {
    }

    public static IMarshalShapeGenerator Instance { get; } = new EmptyMarshalShapeGenerator();

    public bool UsesNativeIdentifier => false;

    public TypeSyntax GetNativeType(IdentifierStubContext context)
    {
        return TypeSyntaxFactory.TypeNameGlobal(context.ManagedType);
    }

    public IEnumerable<StatementSyntax> Generate(MarshalPhase phase, IdentifierStubContext context)
    {
        return [];
    }
}