namespace SashManaged.OpenMp;

[OpenMpApi2(typeof(IExtensible))]
public readonly partial struct IConfig
{
    // TODO: ref return not yet supported

    public partial StringView GetString(StringView key);
    
    // public partial ref int GetInt(StringView key);
    public partial BlittableRef<int> GetInt(StringView key);
    
    //public partial ref float GetFloat(StringView key);
    public partial BlittableRef<float> GetFloat(StringView key);

    public partial Size GetStrings(StringView key, SpanLite<StringView> output);

    public partial Size GetStringsCount(StringView key);
    
    [OpenMpApiFunction("getType")]
    public partial ConfigOptionType GetValueType(StringView key);

    public partial Size GetBansCount();

    // TODO: return by ref [return: OpenMpApiMarshall]
    public partial BanEntry GetBan(Size index);

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

    // public partial ref bool GetBool(StringView key);
    public partial BlittableRef<bool> GetBool(StringView key);
}