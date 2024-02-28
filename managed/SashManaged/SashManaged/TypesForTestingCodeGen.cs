using SashManaged.OpenMp;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
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
public partial interface ICoreEventHandler2 : IEventHandler2
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

[OpenMpEventHandler2(NativeTypeName = "ActorEventHandler")]
public partial interface IActorEventHandler2 : IEventHandler2
{
    void OnPlayerGiveDamageActor(IPlayer player, IActor actor, float amount, uint weapon, BodyPart part);
    void OnActorStreamOut(IActor actor, IPlayer forPlayer);
    void OnActorStreamIn(IActor actor, IPlayer forPlayer);
}

[OpenMpApi2(NativeTypeName = "ICore")]
public readonly partial struct ICore2
{
    public partial SemanticVersion GetVersion();

    public partial int GetNetworkBitStreamVersion();

    public partial IPlayerPool GetPlayers();

    public partial IEventDispatcher2<ICoreEventHandler2> GetEventDispatcher();

    public partial IConfig GetConfig();

    public partial uint GetTickCount();

    public partial void SetGravity(float gravity);

    public partial float GetGravity();

    public partial void SetWeather(int weather);

    public partial void SetWorldTime(Hours time);

    public partial void UseStuntBonuses(bool enable);

    public partial void SetData(SettableCoreDataType type, string data);

    public partial void SetThreadSleep(Microseconds value);

    public partial void UseDynTicks(bool enable);

    public partial void ResetAll();

    public partial void ReloadAll();

    public partial string GetWeaponName(PlayerWeapon weapon);

    public partial void ConnectBot(string name, string script);

    public partial uint TickRate();

    public partial string GetVersionHash();
}