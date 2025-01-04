namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct IConfig
{
    public partial string GetString(string key);
    
    public partial ref int GetInt(string key);
    
    public partial ref float GetFloat(string key);

    public partial Size GetStrings(string key, SpanLite<StringView> output);

    public partial Size GetStringsCount(string key);
    
    [OpenMpApiFunction("getType")]
    public partial ConfigOptionType GetValueType(string key);

    public partial Size GetBansCount();

    public partial BanEntry GetBan(Size index);

    public partial void AddBan(BanEntry entry);
    
    [OpenMpApiOverload("_index")]
    public partial void RemoveBan(Size index);

    public partial void RemoveBan(BanEntry entry);

    public partial void WriteBans();

    public partial void ReloadBans();

    public partial void ClearBans();

    public partial bool IsBanned(BanEntry entry);

    public partial Pair<BlittableBoolean, StringView> GetNameFromAlias(string alias);

    // TODO: public partial void enumOptions(OptionEnumeratorCallback& callback); // enumerator callback not available

    public partial ref bool GetBool(string key);
}