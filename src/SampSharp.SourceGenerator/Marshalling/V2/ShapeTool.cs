namespace SampSharp.SourceGenerator.Marshalling.V2;

/// <summary>
/// Provides methods for determining the shape of a custom marshaller.
/// </summary>
public static class ShapeTool
{
    //
    // stages:
    // during context building:
    // 1. Decide which marshaller implementation to use based on entry point of the specified custom marshaller ( CustomMarshallerTypeFinder )
    // 2. Deduce shape based on the implementation ( ShapeTool )
    // during generation:
    // 3. Activate ShapeGenerator based on shape (CustomMarshalGeneratorFactory.Create)
    // 4. Generate marshaling code and combine with invocation code
    //

    public static MarshallerShape GetShapeOfMarshaller(CustomMarshallerInfo marshaller)
    {
        var methods = MarshalInspector.GetMembers(marshaller);

        var shape = MarshallerShape.None;
        if (marshaller.IsStateless)
        {
            if (methods.StatelessConvertToManagedFinallyMethod != null)
            {
                shape |= MarshallerShape.GuaranteedUnmarshal;
            }

            if (methods.StatelessConvertToManagedMethod != null)
            {
                shape |= MarshallerShape.ToManaged;
            }

            if (methods.StatelessConvertToUnmanagedWithBufferMethod != null)
            {
                shape |= MarshallerShape.CallerAllocatedBuffer | MarshallerShape.ToUnmanaged;
            }

            if (methods.StatelessConvertToUnmanagedMethod != null)
            {
                shape |= MarshallerShape.ToUnmanaged;
            }

            if (methods.StatelessFreeMethod != null)
            {
                shape |= MarshallerShape.Free;
            }

            if (methods.StatelessGetPinnableReferenceMethod != null)
            {
                shape |= MarshallerShape.StatelessPinnableReference;
            }
        }
        else if (marshaller.IsStateful)
        {
            if (methods.StatefulToUnmanagedMethod != null)
            {
                shape |= MarshallerShape.ToUnmanaged;

                if (methods.StatefulFromManagedWithBufferMethod != null)
                {
                    shape |= MarshallerShape.CallerAllocatedBuffer;
                }
            }

            if (methods.StatefulToManagedMethod != null)
            {
                shape |= MarshallerShape.ToManaged;
            }

            if (methods.StatefulFreeMethod != null)
            {
                shape |= MarshallerShape.Free;
            }

            if (methods.StatefulOnInvokedMethod != null)
            {
                shape |= MarshallerShape.OnInvoked;
            }

            if (methods.StatefulGetPinnableReferenceMethod != null)
            {
                shape |= MarshallerShape.StatefulPinnableReference;
            }

            if (methods.StatelessGetPinnableReferenceMethod != null)
            {
                shape |= MarshallerShape.StatelessPinnableReference;
            }
        }

        return shape;
    }
}