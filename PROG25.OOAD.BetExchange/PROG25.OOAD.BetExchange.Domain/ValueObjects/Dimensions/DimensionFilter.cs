using System.Collections.Immutable;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public record DimensionFilter
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

    public ImmutableDictionary<string, object> Value { get; }
    public DimensionDefinition Definition { get; }
}
