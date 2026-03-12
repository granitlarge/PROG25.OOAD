using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Extensions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public class DimensionFilter : IEquatable<DimensionFilter>
{
    public DimensionFilter
    (
        ImmutableDictionary<string, object> value,
        DimensionDefinition definition
    )
    {
        if (value.Count > definition.NameToTypeMapping.Count)
        {
            throw new ArgumentException("Query contained more parameters than possible");
        }

        var mismatchedKey = value.Keys.FirstOrDefault(k => !definition.NameToTypeMapping.ContainsKey(k));
        if (mismatchedKey != null)
        {
            throw new ArgumentException($"Key {mismatchedKey} does not exist in dimension {definition}");
        }

        var mismatchedType = value.FirstOrDefault(kv => definition.NameToTypeMapping[kv.Key] != kv.Value.GetType());
        if (mismatchedType.Key != null)
        {
            throw new ArgumentException($"Value with key {mismatchedType.Key} must be of type {definition.NameToTypeMapping[mismatchedType.Key]} but it was of type {mismatchedType.Value}");
        }

        Value = value;
        Definition = definition;
    }

    private int? _hashCode;

    public ImmutableDictionary<string, object> Value { get; }
    public DimensionDefinition Definition { get; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DimensionFilter);
    }

    public bool Equals(DimensionFilter? df)
    {
        if (df is null)
            return false;

        return Definition == df.Definition &&
               Value.Count == df.Value.Count &&
               Value.All(df.Value.Contains) &&
               df.Value.All(Value.Contains);
    }

    public override int GetHashCode()
    {
        return _hashCode ??= HashCode.Combine(Definition, Value.CalculateHashCode());
    }

    public static bool operator ==(DimensionFilter left, DimensionFilter right)
    {
        return left.GetHashCode() == right.GetHashCode() && left.Equals(right);
    }

    public static bool operator !=(DimensionFilter left, DimensionFilter right)
    {
        return !(left == right);
    }
}
