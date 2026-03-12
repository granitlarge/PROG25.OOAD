using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Extensions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public class DimensionDefinition : IEquatable<DimensionDefinition>
{
    private int? _hashCode = null;

    public DimensionDefinition
    (
        ImmutableDictionary<string, Type> nameToTypeMapping
    )
    {
        NameToTypeMapping = nameToTypeMapping;
    }

    public ImmutableDictionary<string, Type> NameToTypeMapping { get; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DimensionDefinition);
    }

    public bool Equals(DimensionDefinition? dd)
    {
        if (dd is null) return false;

        return NameToTypeMapping.Count == dd.NameToTypeMapping.Count &&
            NameToTypeMapping.All(dd.NameToTypeMapping.Contains) &&
            dd.NameToTypeMapping.All(NameToTypeMapping.Contains);
    }

    public override int GetHashCode()
    {
        return _hashCode ??= NameToTypeMapping.CalculateHashCode();
    }

    public override string ToString()
    {
        return NameToTypeMapping.ToString() ?? "IDK WHY THIS IS NULL BRO.";
    }

    public static bool operator ==(DimensionDefinition left, DimensionDefinition right)
    {
        return left.GetHashCode() == right.GetHashCode() && left.Equals(right);
    }

    public static bool operator !=(DimensionDefinition left, DimensionDefinition right)
    {
        return !(left == right);
    }
}