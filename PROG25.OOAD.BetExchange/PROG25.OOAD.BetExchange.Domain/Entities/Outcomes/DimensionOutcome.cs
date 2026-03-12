using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;

namespace PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;

public class DimensionOutcome : Outcome
{
    public DimensionOutcome(DimensionFilter dimensionFilter) : base(string.Join(" and ", dimensionFilter.Value.Select(kv => $"{kv.Key}={kv.Value}")))
    {
        DimensionFilter = dimensionFilter;
    }

    public DimensionFilter DimensionFilter { get; }
}