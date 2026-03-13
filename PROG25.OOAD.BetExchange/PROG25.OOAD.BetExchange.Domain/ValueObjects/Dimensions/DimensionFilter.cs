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
            throw new ArgumentException($"Value with key '{mismatchedType.Key}' must be of type '{definition.NameToTypeMapping[mismatchedType.Key]}' but it was of type '{mismatchedType.Value.GetType()}'");
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

    public DimensionFilter CombineWith(DimensionFilter other)
    {
        if (Definition != other.Definition)
        {
            throw new ArgumentException("Cannot combine dimension filters of different definitions.");
        }

        var distinctKeysThis = Value.Keys.ToImmutableHashSet();
        var distinctKeysOthers = other.Value.Keys.ToImmutableHashSet();

        var duplicateKeys = distinctKeysThis.Any(distinctKeysOthers.Contains) ||
                            distinctKeysOthers.Any(distinctKeysThis.Contains);

        if (duplicateKeys)
        {
            throw new ArgumentException("Other contains keys which this filter contains, it is impossible to choose a value.");
        }

        return new DimensionFilter
        (
            Value.Concat(other.Value).ToImmutableDictionary(),
            Definition
        );
    }
}
