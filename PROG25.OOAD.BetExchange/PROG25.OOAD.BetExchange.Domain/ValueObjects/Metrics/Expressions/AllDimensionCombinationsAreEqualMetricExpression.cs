using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record AllDimensionCombinationsAreEqualMetricExpression
 : MetricExpression<bool>
{
    public AllDimensionCombinationsAreEqualMetricExpression
    (
        MetricDefinition definition,
        ImmutableHashSet<DimensionFilter> filters,
        ImmutableHashSet<string> dimensions,
        int expectedDimensionCombinations
    )
    {
        foreach (var filter in filters)
        {
            if (!definition.IsValidDimensionFilter(filter))
            {
                throw new ArgumentException($"Dimensionfilter {filter} is invalid for metric definition {definition.Name}");
            }
        }

        foreach (var dimension in dimensions)
        {
            if (!definition.Dimension.NameToTypeMapping.ContainsKey(dimension))
            {
                throw new ArgumentException($"Dimension {dimension} is invalid for metric definition {definition.Name}");
            }
        }

        if (expectedDimensionCombinations < 2)
        {
            throw new ArgumentException($"'{nameof(expectedDimensionCombinations)}' must be at least 2.");
        }

        Definition = definition;
        Filters = filters;
        Dimensions = dimensions;
        ExpectedDimensionCombinations = expectedDimensionCombinations;
    }

    public MetricDefinition Definition { get; }
    public ImmutableHashSet<DimensionFilter> Filters { get; }
    public ImmutableHashSet<string> Dimensions { get; }
    public int ExpectedDimensionCombinations { get; }

    public override bool Evaluate(EventMetrics metrics)
    {
        var metricValues = metrics.GetByDefinition(Definition);
        var grouped = Definition.GroupBy(Dimensions, metricValues);
        if (grouped.Count < ExpectedDimensionCombinations)
        {
            return false;
        }

        if (grouped.Count > ExpectedDimensionCombinations)
        {
            throw new InvalidOperationException($"Expected exactly {ExpectedDimensionCombinations} dimension combinations but got {grouped.Count}");
        }

        return grouped.Select(g => Definition.Aggregate(g.Values)).Distinct().Count() == 1;
    }
}