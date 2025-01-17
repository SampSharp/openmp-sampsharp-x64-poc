﻿using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using SampSharp.OpenMp.Core.Api;

namespace SampSharp.OpenMp.Core;


public partial class Testing
{
    // [LibraryImport("SampSharp")]
    // public static partial int WithRefString([MarshalUsing(typeof(StringViewMarshaller))] ref string str);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithRefReadonlyString([MarshalUsing(typeof(StringViewMarshaller))] ref readonly string str);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithRealInString([MarshalUsing(typeof(StringViewMarshaller))] in string str);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithInString([MarshalUsing(typeof(StringViewMarshaller))] string str);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithOutString([MarshalUsing(typeof(StringViewMarshaller))] out string str);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithDefaultMarshaller([MarshalUsing(typeof(BooleanMarshaller))]ref bool b);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int WithToManagedFinallyAndOnInvoked([MarshalUsing(typeof(SafeHandleMarshaller<SafeHandle>))] ref SafeHandle ptr, SettableCoreDataType type);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int FooTestIn([MarshalUsing(typeof(FooMarshaller))] Foo ptr, SettableCoreDataType type);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int FooTestIn2([MarshalUsing(typeof(FooMarshaller))] in Foo ptr, SettableCoreDataType type);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int FooTestIn3([MarshalUsing(typeof(FooMarshaller))] Foo ptr, [MarshalUsing(typeof(FooMarshaller))] Foo ptr3, [MarshalUsing(typeof(FooMarshaller))] in Foo ptr2, SettableCoreDataType type, [MarshalUsing(typeof(StringViewMarshaller))] string str);
    //
    // [LibraryImport("SampSharp")]
    // [return: MarshalUsing(typeof(FooMarshaller))] public static partial Foo FooTestOut(SettableCoreDataType type);
    //
    // [LibraryImport("SampSharp")]
    // public static partial int FooTestRef([MarshalUsing(typeof(FooMarshaller))] ref Foo ptr, SettableCoreDataType type);
    //
    // [LibraryImport("SampSharp")]
    // public static partial BarStruct GetBar();
    
    [LibraryImport("SampSharp")]
    public static partial int TestString([MarshalUsing(typeof(StringViewMarshaller))] string message);
    [LibraryImport("SampSharp")]
    public static partial int TestInString([MarshalUsing(typeof(StringViewMarshaller))] in string message);
    [LibraryImport("SampSharp")]
    public static partial int TestOutString([MarshalUsing(typeof(StringViewMarshaller))] out string message);
    [LibraryImport("SampSharp")]
    public static partial int TestRefString([MarshalUsing(typeof(StringViewMarshaller))] ref string str);
    [LibraryImport("SampSharp")]
    public static partial int TestRefReadonlyString([MarshalUsing(typeof(StringViewMarshaller))] ref readonly string str);
    
    [LibraryImport("SampSharp")]
    public static partial void ApplyAnimation(BlittableStructRef<AnimationDataMarshaller.Native> animation, PlayerAnimationSyncType syncType);


    public delegate void MyDelegate([MarshalUsing(typeof(StringViewMarshaller))]string myString);

    [LibraryImport("SampSharp")]
    public static partial void SomethingWithDelegate(MyDelegate del);
    
    [LibraryImport("SampSharp")]
    public static partial void Burp([MarshalUsing(typeof(MicrosecondsMarshaller))]TimeSpan value);

}

[CustomMarshaller(typeof(Foo), MarshalMode.ManagedToUnmanagedIn, typeof(SFManagedToUnmanagedInPin))]
[CustomMarshaller(typeof(Foo), MarshalMode.ManagedToUnmanagedOut, typeof(SFManagedToUnmanagedOutFinally))]
[CustomMarshaller(typeof(Foo), MarshalMode.ManagedToUnmanagedRef, typeof(ManagedToNativeRefGUPin))]
public static class FooMarshaller
{
    public static unsafe class ManagedToNativeIn
    {
        public static nint ConvertToUnmanaged(Foo managed)
        {
            return 3;
        }

        public static ref byte* GetPinnableReference(Foo managed)
        {
            throw new NotImplementedException();
        }
        
        public static void Free(byte* unmanaged)
        {

        }
        public static void Free(nint unmanaged)
        {

        }
    }
    
    public static unsafe class ManagedToNativeInBuffer
    {
        public static int BufferSize => 33;

        public static nint ConvertToUnmanaged(Foo managed, Span<byte> callerAllocatedBuffer)
        {
            return 0;
        }

        public static void Free(nint unmanaged)
        {

        }
    }

    public static class ManagedToNativeOut
    {
        public static Foo ConvertToManaged(nint unmanaged)
        {
            return new Foo();
        }

        public static void Free(nint unmanaged)
        {
        
        }
    }

    public static class ManagedToNativeOutGuarenteedUnmarshalling
    {
        public static Foo ConvertToManagedFinally(nint unmanaged)
        {
            return new Foo();
        }

        public static void Free(nint unmanaged)
        {

        }
    }

    public static unsafe class ManagedToNativeRefGUPin
    {
        public static nint ConvertToUnmanaged(Foo managed)
        {
            return 3;
        }

        public static ref byte* GetPinnableReference(Foo managed)
        {
            throw new NotImplementedException();
        }

        public static Foo ConvertToManagedFinally(nint unmanaged)
        {
            return new Foo();
        }

        public static void Free(nint unmanaged)
        {

        }
    }

    public unsafe ref struct SFManagedToUnmanagedInPin 
    {
        public SFManagedToUnmanagedInPin()
        {
        }

        public void FromManaged(Foo managed)
        {

        }

        public ref byte* GetPinnableReference()
        {
            throw new NotImplementedException();
        }

        public static ref byte* GetPinnableReference(Foo managed)
        {
            // passed to invoke
            throw new NotImplementedException();
        }

        public nint ToUnmanaged()
        {
            return 0;
        }

        public void OnInvoked(){}

        public void Free(){}
    }

    public ref struct SFManagedToUnmanagedOutFinally
    {
        public SFManagedToUnmanagedOutFinally()
        {

        }

        public void FromUnmanaged(nint unmanaged)
        {

        }


        public Foo ToManagedFinally()
        {
            return new Foo();
        }

        public void Free()
        {

        }
    }
}

public struct BarStruct
{
    public int A;
    public int B;
    public int C;
}

public  unsafe partial class Foo
{
    public static bool _test;


    public static ref bool Test()
    {
        return ref _test;
    }
    
    [LibraryImport("SampSharp")]
    private static partial bool* Fld();

    public static ref bool Test2()
    {
        bool* fld;

        fld = Fld();

        return ref *fld;
    }
}

[OpenMpApi]
public readonly partial struct BaseTest
{
    public partial int GetSomeNumber();

    public partial void SetSomeNumber(int num);
}

[OpenMpApi(typeof(BaseTest), Library = "FooLib")]
public readonly partial struct TestV2
{
    public partial int TestString(string message);
    public partial int TestInString(in string message);
    public partial int TestOutString(out string message);
    public partial int TestRefString(ref string str);
    public partial int TestRefReadonlyString(ref readonly string str);
    public partial void ApplyAnimation(BlittableStructRef<AnimationDataMarshaller.Native> animation, PlayerAnimationSyncType syncType);

    // public partial string TestReturnString();
    // public partial int FooTestIn([MarshalUsing(typeof(FooMarshaller))] Foo ptr, SettableCoreDataType type);
    // public partial int FooTestIn2([MarshalUsing(typeof(FooMarshaller))] in Foo ptr, SettableCoreDataType type);
    // [return: MarshalUsing(typeof(FooMarshaller))] public partial Foo FooTestOut(SettableCoreDataType type);
    // public partial int FooTestRef([MarshalUsing(typeof(FooMarshaller))] ref Foo ptr, SettableCoreDataType type);

}

[OpenMpApi(typeof(IComponent))]
public readonly partial struct Ff
{
    public static UID ComponentId => new();
}

public class Cc
{
    public void C()
    {
        Ff f = new Ff(0);

        var ptr = (IComponent)f;
        var name = ptr.ComponentName();

        var name2= f.ComponentName();
    }
}
