using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SashManaged.SourceGenerator.Marshalling;

public class BidirectionalMarshallerShape(IMarshallerShape inShape, IMarshallerShape outShape) : IMarshallerShape
{
    public TypeSyntax GetNativeType()
    {
        return inShape.GetNativeType();
    }

    public SyntaxList<StatementSyntax> Setup(IParameterSymbol? parameterSymbol)
    {
        // marshaller setups should be the same for both shapes
        return inShape.Setup(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> Marshal(IParameterSymbol? parameterSymbol)
    {
        // only managed -> unmanaged marshaller marshal
        return inShape.Marshal(parameterSymbol).AddRange(outShape.Marshal(parameterSymbol));
    }

    public SyntaxList<StatementSyntax> PinnedMarshal(IParameterSymbol? parameterSymbol)
    {
        // only managed -> unmanaged marshaller marshal
        return inShape.PinnedMarshal(parameterSymbol);
    }

    public FixedStatementSyntax? Pin(IParameterSymbol? parameterSymbol)
    {
        // only managed -> unmanaged marshaller should pin
        return inShape.Pin(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> UnmarshalCapture(IParameterSymbol? parameterSymbol)
    {
        // only unmanaged -> managed should unmarshal
        return outShape.UnmarshalCapture(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> Unmarshal(IParameterSymbol? parameterSymbol)
    {
        // only unmanaged -> managed should unmarshal
        return outShape.Unmarshal(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> CleanupCallerAllocated(IParameterSymbol? parameterSymbol)
    {
        // both shapes should share the same cleanup signature, therefore only one shape needs to be added to the syntax node
        return inShape.CleanupCallerAllocated(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> CleanupCalleeAllocated(IParameterSymbol? parameterSymbol)
    {
        return default;
    }

    public SyntaxList<StatementSyntax> NotifyForSuccessfulInvoke(IParameterSymbol? parameterSymbol)
    {
        // only managed -> unmanaged marshaller should be notified
        return inShape.NotifyForSuccessfulInvoke(parameterSymbol);
    }

    public SyntaxList<StatementSyntax> GuaranteedUnmarshal(IParameterSymbol? parameterSymbol)
    {
        // only unmanaged -> managed should unmarshal
        return outShape.GuaranteedUnmarshal(parameterSymbol);
    }

    public bool RequiresLocal => inShape.RequiresLocal || outShape.RequiresLocal;

    public ArgumentSyntax GetArgument(ParameterStubGenerationContext ctx)
    {
        return inShape.GetArgument(ctx);
    }
}