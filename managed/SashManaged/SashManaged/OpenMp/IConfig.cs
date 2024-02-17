namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct IConfig
{
    public partial StringView GetString(StringView key);

    public partial ref int GetInt(StringView key);

    public partial ref float GetFloat(StringView key);

    public partial Size GetStrings(StringView key, SpanLite<StringView> output);

    public partial Size GetStringsCount(StringView key);
    
    [OpenMpApiFunction("getType")]
    public partial ConfigOptionType GetValueType(StringView key);

    public partial Size GetBansCount();

    [return: OpenMpApiMarshall]
    public partial BanEntry GetBan(Size index);

    public partial void AddBan([OpenMpApiMarshall] BanEntry entry);

    [OpenMpApiOverload("_index")]
    public partial void RemoveBan(Size index);

    public partial void RemoveBan([OpenMpApiMarshall] BanEntry entry);

    public partial void WriteBans();

    public partial void ReloadBans();

    public partial void ClearBans();

    public partial bool IsBanned([OpenMpApiMarshall] BanEntry entry);

    public partial Pair<BlittableBoolean, StringView> GetNameFromAlias(StringView alias);

    // TODO: public partial void enumOptions(OptionEnumeratorCallback& callback); // enumerator callback not available

    public partial ref bool GetBool(StringView key);
}