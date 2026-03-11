using System.Collections.Immutable;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public record DimensionValue
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

    public DimensionDefinition Definition { get; }
    public ImmutableDictionary<string, object> Value { get; }
}
