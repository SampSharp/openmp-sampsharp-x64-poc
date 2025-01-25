using System.Runtime.InteropServices;

namespace SampSharp.OpenMp.Core.Api;

[OpenMpApi(typeof(IExtensible))]
public readonly partial struct IConfig
{
    public partial string GetString(string key);
    
    public partial ref int GetInt(string key);
    
    public partial ref float GetFloat(string key);

    [OpenMpApiFunction("getStrings")]
    private partial Size GetStringsImpl(string key, SpanLite<StringView> output);

    private partial Size GetStringsCount(string key);

    public unsafe string?[] GetStrings(string key)
    {
        var count = GetStringsCount(key);

        var ptr = Marshal.AllocHGlobal(count.Value.ToInt32() * sizeof(StringView));

        try
        {
            var output = new SpanLite<StringView>((StringView*)ptr, count);
            GetStringsImpl(key, output);

            var result = new string?[count.Value.ToInt32()];
            var index = 0;
            foreach (var value in output.AsSpan())
            {
                result[index++] = value;
            }

            return result;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
    
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

    [OpenMpApiFunction("getNameFromAlias")]
    private partial void GetNameFromAliasImpl(string alias, out Pair<BlittableBoolean, StringView> result);

    public (bool, string?) GetNameFromAlias(string alias)
    {
        GetNameFromAliasImpl(alias, out var pair);
        return (pair.First, pair.Second);
    }

    // TODO: public partial void enumOptions(OptionEnumeratorCallback& callback); // enumerator callback not available

    public partial ref bool GetBool(string key);
}