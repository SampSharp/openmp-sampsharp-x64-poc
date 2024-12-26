using SashManaged.OpenMp;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SashManaged;


public partial class Testing
{
    [LibraryImport("SampSharp")]
    public static partial int WithRefString([MarshalUsing(typeof(StringViewMarshaller))] ref string str);
    
    [LibraryImport("SampSharp")]
    public static partial int WithInString([MarshalUsing(typeof(StringViewMarshaller))] string str);

    [LibraryImport("SampSharp")]
    public static partial int WithOutString([MarshalUsing(typeof(StringViewMarshaller))] out string str);

    [LibraryImport("SampSharp")]
    public static partial int WithDefaultMarshaller([MarshalUsing(typeof(BooleanMarshaller))]ref bool b);

    [LibraryImport("SampSharp")]
    public static partial int WithToManagedFinallyAndOnInvoked([MarshalUsing(typeof(SafeHandleMarshaller<SafeHandle>))] ref SafeHandle ptr, SettableCoreDataType type);
}

[OpenMpApi2]
public readonly partial struct BaseTest
{
    public partial int GetSomeNumber();

    public partial void SetSomeNumber(int num);
}

[OpenMpApi2(typeof(BaseTest), Library = "FooLib")]
public readonly partial struct TestV2
{
    public partial int TestInBool([MarshalUsing(typeof(BooleanMarshaller))] bool b);

    public partial int TestInString(int style, string message, ref Milliseconds time, ref Milliseconds remaining);
    public partial int TestRefString(int style, ref string message, ref Milliseconds time, ref Milliseconds remaining);
    public partial int TestOutString(int style, out string message, ref Milliseconds time, ref Milliseconds remaining);

    public partial string TestReturnString();

}

[OpenMpEventHandler2(NativeTypeName = "CoreEventHandler")]
public partial interface ICoreEventHandler2
{
    void OnTick(Microseconds micros, TimePoint now);

    // void OnText(int text); // no marshalling support yet
    //
    // should marshal like: 
    // Delegate __onText_delegate = (OnText_)((text) =>
    // {
    //     var __text_managed = StringViewMarshaller.ConvertToManaged(text)!;
    //     OnText(__text_managed);
    // });
}

[OpenMpApi2(typeof(IComponent))]
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

[OpenMpEventHandler2(NativeTypeName = "ActorEventHandler")]
public partial interface IActorEventHandler2 : IEventHandler2
{
    void OnPlayerGiveDamageActor(IPlayer player, IActor actor, float amount, uint weapon, BodyPart part);
    void OnActorStreamOut(IActor actor, IPlayer forPlayer);
    void OnActorStreamIn(IActor actor, IPlayer forPlayer);
}