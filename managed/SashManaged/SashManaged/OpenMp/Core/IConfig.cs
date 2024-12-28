namespace SashManaged.OpenMp;

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

    public partial BlittableStructRef<BanEntry> GetBan(Size index);

    public partial void AddBan(in BanEntry entry);

    [OpenMpApiOverload("_index")]
    public partial void RemoveBan(Size index);

    public partial void RemoveBan(in BanEntry entry);

    public partial void WriteBans();

    public partial void ReloadBans();

    public partial void ClearBans();

    public partial bool IsBanned(in BanEntry entry);

    public partial Pair<BlittableBoolean, StringView> GetNameFromAlias(StringView alias);

    // TODO: public partial void enumOptions(OptionEnumeratorCallback& callback); // enumerator callback not available

    public partial ref bool GetBool(string key);
}