using System;
using System.Collections.Generic;
using System.Text;

namespace SampSharp.SourceGenerator.Helpers;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
    {
        foreach (var item in source)
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }
}
