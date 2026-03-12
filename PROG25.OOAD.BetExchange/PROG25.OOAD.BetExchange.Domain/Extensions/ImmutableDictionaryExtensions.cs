using System.Collections.Immutable;

namespace PROG25.OOAD.BetExchange.Domain.Extensions;

public static class ImmutableDictionaryExtensions
{
    public static int CalculateHashCode<T_Key, T_Value>(this ImmutableDictionary<T_Key, T_Value> value) where T_Key : notnull
    {
        var hashcode = 0;
        foreach (var kv in value)
        {
            hashcode ^= HashCode.Combine(hashcode, kv.Key, kv.Value);
        }
        return hashcode;
    }
}