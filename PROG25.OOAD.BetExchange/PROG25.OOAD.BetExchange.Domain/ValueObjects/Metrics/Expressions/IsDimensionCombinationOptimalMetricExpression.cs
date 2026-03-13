using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record IsDimensionCombinationOptimalMetricExpression : MetricExpression<bool>
{
    public IsDimensionCombinationOptimalMetricExpression
    (
        MetricDefinition definition,
        ImmutableHashSet<DimensionFilter> filters,
        DimensionFilter optimumFilter,
        OptimumType optimumType
    )
    {
        Definition = definition;
        Filters = filters;
        OptimumFilter = optimumFilter;
        OptimumType = optimumType;
    }

    public MetricDefinition Definition { get; }
    public ImmutableHashSet<DimensionFilter> Filters { get; }
    public DimensionFilter OptimumFilter { get; }
    public OptimumType OptimumType { get; }

    // The correctness of this code depends on the ACTUAL number of dimension combinations.
    // If there's only one combination of dimensions (not in the data, but theoretically), then its incorrect 
    // (but then again, why would you check whether its optimal among all dimension combinations if only one combination exists)
    // If there are two combinations or more it is correct.

    public override bool Evaluate(EventMetrics metrics)
    {
        var relevantMetrics = metrics.GetByDefinition(Definition);
        var filtered = Definition.Filter(Filters, relevantMetrics);
        var grouped = Definition.GroupBy([.. OptimumFilter.Value.Select(kv => kv.Key)], filtered);

        // If there are no dimension combinations in the data then the 
        // metric value of all those combinations is 0, hence there is no optimum.
        if (grouped.Count == 0)
        {
            return false;
        }

        // If there's only 1 dimension combination in the data, then it is the optimum.
        if (grouped.Count == 1)
        {
            return grouped.Single().Key == OptimumFilter;
        }

        // If the supposed optimum doesn't exist in the data, then its not the optimum.
        if (grouped.All(g => g.Key != OptimumFilter))
        {
            return false;
        }

        var aggregated = grouped.Select(g => (g.Key, Value: Definition.Aggregate(g.Values)));
        var supposedOptimumValue = aggregated.SingleOrDefault(g => g.Key == OptimumFilter).Value;

        // Otherwise it IS the optimum if its greater than than the maximum metric value of all other dimension combinations.
        return OptimumType switch
        {
            OptimumType.Maximum => aggregated.Where(g => g.Key != OptimumFilter).Max(e => e.Value) < supposedOptimumValue,
            OptimumType.Minimum => aggregated.Where(g => g.Key != OptimumFilter).Min(e => e.Value) > supposedOptimumValue,
            _ => throw new NotImplementedException($"Unknown OptimumType: {OptimumType}")
        };
    }
}