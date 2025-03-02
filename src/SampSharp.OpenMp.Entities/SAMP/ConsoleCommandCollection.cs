using System.Collections;
using SampSharp.OpenMp.Core.RobinHood;

namespace SampSharp.Entities.SAMP;

[EventParameter]
public class ConsoleCommandCollection : IReadOnlyCollection<string>
{
    private readonly FlatHashSetStringView _set;

    public ConsoleCommandCollection(FlatHashSetStringView set)
    {
        _set = set;
    }
    
    public int Count => _set.Count;

    public IEnumerator<string> GetEnumerator()
    {
        return _set.Select(item => item ?? string.Empty).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(string command)
    {
        _set.Emplace(command);
    }
}