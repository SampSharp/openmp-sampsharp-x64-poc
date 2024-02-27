using SashManaged.OpenMp;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SashManaged;


public partial class Testing
{
    //[System.Runtime.InteropServices.DllImport("SampSharp", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
    
    //[LibraryImport("SampSharp")]
    //public static partial IVehicle IVehiclesComponent_create(IVehiclesComponent ptr, BlittableBoolean isStatic, int modelID, Vector3 position, float Z, int colour1, int colour2, int respawnDelay, BlittableBoolean addSiren);
    
    [LibraryImport("SampSharp")]
    public static partial int WithRefString([MarshalUsing(typeof(StringViewMarshaller))] ref string str);
    
    [LibraryImport("SampSharp")]
    public static partial int WithInString([MarshalUsing(typeof(StringViewMarshaller))] string str);

    [LibraryImport("SampSharp")]
    public static partial int WithOutString([MarshalUsing(typeof(StringViewMarshaller))] out string str);

    [LibraryImport("SampSharp")]
    public static partial int WithDefaultMarshaller([MarshalUsing(typeof(BooleanMarshaller))]ref bool b);

    [LibraryImport("SampSharp")]
    public static partial int WithToManagedFinallyAndOnInvoked([MarshalUsing(typeof(SafeHandleMarshaller<SafeHandle>))] ref SafeHandle ptr, SashManaged.OpenMp.SettableCoreDataType type);
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

[OpenMpApi2(NativeTypeName = "ICore")]
public readonly partial struct ICore2
{
    public partial SemanticVersion GetVersion();

    public partial int GetNetworkBitStreamVersion();

    public partial IPlayerPool GetPlayers();

    public partial IEventDispatcher<ICoreEventHandler> GetEventDispatcher();

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