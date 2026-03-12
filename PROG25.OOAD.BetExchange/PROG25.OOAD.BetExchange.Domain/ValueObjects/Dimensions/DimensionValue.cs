using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Extensions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public class DimensionValue : IEquatable<DimensionValue>
{
    public DimensionValue(ImmutableDictionary<string, object> value, DimensionDefinition definition)
    {
        if (value.Count != definition.NameToTypeMapping.Count)
        {
            throw new ArgumentException($"Dimension value has smaller cardinality ({value.Count}) than expected ({definition.NameToTypeMapping.Count}) by the definition");
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

        Definition = definition;
        Value = value;
    }

    private int? _hashCode;

    public DimensionDefinition Definition { get; }
    public ImmutableDictionary<string, object> Value { get; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DimensionValue);
    }

    public override int GetHashCode()
    {
        return _hashCode ??= HashCode.Combine(Definition, Value.CalculateHashCode());
    }

    public bool Equals(DimensionValue? dv)
    {
        if (dv is null)
            return false;
        return
            Definition == dv.Definition &&
            Value.Count == dv.Value.Count &&
            Value.All(dv.Value.Contains) && dv.Value.All(Value.Contains);
    }

    public static bool operator ==(DimensionValue left, DimensionValue right)
    {
        return left.GetHashCode() == right.GetHashCode() && left.Equals(right);
    }

    public static bool operator !=(DimensionValue left, DimensionValue right)
    {
        return !(left == right);
    }
}