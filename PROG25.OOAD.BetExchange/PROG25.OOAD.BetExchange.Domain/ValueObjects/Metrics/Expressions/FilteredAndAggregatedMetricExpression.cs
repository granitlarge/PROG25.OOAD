using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record FilteredAndAggregatedMetricExpression : MetricExpression<decimal>
{
    public FilteredAndAggregatedMetricExpression(MetricDefinition definition, ImmutableHashSet<DimensionFilter> filters)
    {
        foreach (var filter in filters)
        {
            if (!definition.IsValidDimensionFilter(filter))
            {
                throw new ArgumentException($"Dimension filter '{filter}' is not valid for metric '{definition.Name}'.", nameof(filters));
            }
        }

        Definition = definition;
        Filters = filters;
    }

    public MetricDefinition Definition { get; }
    public ImmutableHashSet<DimensionFilter> Filters { get; }

    public override decimal Evaluate(EventMetrics metrics)
    {
        var relevantMetricValues = metrics.GetByDefinition(Definition);
        return Definition.Aggregate(Definition.Filter(Filters, relevantMetricValues));
    }
}