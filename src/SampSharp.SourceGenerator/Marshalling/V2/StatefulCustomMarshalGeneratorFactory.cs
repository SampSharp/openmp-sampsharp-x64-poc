using SampSharp.SourceGenerator.Marshalling.V2.ShapeGenerators;

namespace SampSharp.SourceGenerator.Marshalling.V2;

public class StatefulCustomMarshalGeneratorFactory : ICustomMarshalGeneratorFactory
{
    public static ICustomMarshalGeneratorFactory Instance { get; } = new StatefulCustomMarshalGeneratorFactory();

    public IMarshalShapeGenerator Create(MarshallerShape shape)
    {
        var generator = EmptyMarshalShapeGenerator.Instance;
        
        if (shape.HasFlag(MarshallerShape.StatelessPinnableReference))
        {
            return new StaticPinnableManagedValueMarshaller(generator);
        }

        if (shape.HasFlag(MarshallerShape.ToUnmanaged))
        {
            generator = new StatefulManagedToUnmanaged(generator);
        }
        
        if (shape.HasFlag(MarshallerShape.ToManaged))
        {
            generator = new StatefulUnmanagedToManaged(generator);
        }

        if (shape.HasFlag(MarshallerShape.GuaranteedUnmarshal))
        {
            generator = new StatefulGuaranteedUnmarshal(generator);
        }

        if (shape.HasFlag(MarshallerShape.CallerAllocatedBuffer))
        {
            generator = new CallerAllocatedBuffer(generator);
        }

        if(shape.HasFlag(MarshallerShape.ToManaged))
        {
            generator = new StatefulUnmanagedToManaged(generator);
        }

        if (shape.HasFlag(MarshallerShape.Free))
        {
            generator = new StatefulFree(generator);
        }
        
        if(shape.HasFlag(MarshallerShape.OnInvoked))
        {
            generator = new StatefulNotifyForSuccesfulInvokeMarshaller(generator);
        }

        if (shape.HasFlag(MarshallerShape.StatefulPinnableReference))
        {
            return new StatefulPinnableManagedValueMarshaller(generator);
        }

        return generator;
    }
}