namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(ILogger))]
public readonly partial struct ICore
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

    public partial void SetWorldTime(int time);

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