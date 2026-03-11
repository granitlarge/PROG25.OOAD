using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record ComparisonScopedEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public ComparisonScopedEventMetricMarketConfiguration
    (
        decimal referenceValue,
        ImmutableHashSet<DimensionFilter> dimensions,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        ComparisonResult expectedComparisonResult,
        string name
    ) : base(metric, timestamp, name)
    {
        if (!metric.IsValidMetricValue(referenceValue))
        {
            throw new ArgumentException("Reference value is not valid for the metric.", nameof(referenceValue));
        }

        foreach (var dimension in dimensions)
        {
            if (!metric.IsValidDimensionQuery(dimension))
            {
                throw new ArgumentException($"Dimension '{dimension}' is not valid for the metric.", nameof(dimensions));
            }
        }

        ReferenceValue = referenceValue;
        ExpectedComparisonResult = expectedComparisonResult;
        Dimensions = dimensions;
    }

    public decimal ReferenceValue { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public ImmutableHashSet<DimensionFilter> Dimensions { get; }
}