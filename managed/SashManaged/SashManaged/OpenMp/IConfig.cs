namespace SashManaged.OpenMp;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct IConfig
{
    public partial StringView GetString(StringView key);

    public partial ref int GetInt(StringView key);

    public partial ref float GetFloat(StringView key);

    // public partial Size getStrings(StringView key, Span<StringView> output); // TODO: span

    public partial Size GetStringsCount(StringView key);

    public partial ConfigOptionType GetType(StringView key);

    public partial Size GetBansCount();

    [return:Marshall]
    public partial BanEntry GetBan(Size index);

    public partial void AddBan([Marshall]BanEntry entry);

    //public partial void RemoveBan(Size index); // TODO: overloads

    public partial void RemoveBan([Marshall]BanEntry entry);

    public partial void WriteBans();

    public partial void ReloadBans();

    public partial void ClearBans();

    public partial bool IsBanned([Marshall]BanEntry entry);

    public partial PairBoolString GetNameFromAlias(StringView alias);

    // public partial void enumOptions(OptionEnumeratorCallback& callback);

    public partial ref bool GetBool(StringView key);
}