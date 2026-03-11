using System.Collections.Immutable;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

public record DimensionDefinition
{
    public DimensionDefinition(ImmutableDictionary<string, Type> nameToTypeMapping)
    {
        if (nameToTypeMapping.Count == 0)
        {
            throw new ArgumentException("A dimension must have at least one name/type combination");
        }
        NameToTypeMapping = nameToTypeMapping;
    }

    public ImmutableDictionary<string, Type> NameToTypeMapping { get; }
}
