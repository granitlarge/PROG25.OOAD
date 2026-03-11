using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record OptimalDimensionEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public OptimalDimensionEventMetricMarketConfiguration
    (
        DimensionFilter dimension,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name,
        OptimumType optimumType
    ) : base(metric, timestamp, name)
    {
        OptimumType = optimumType;
        if (!metric.IsValidDimensionQuery(dimension))
        {
            throw new ArgumentException($"Dimension '{dimension}' is not valid for the metric.", nameof(dimension));
        }
        Dimension = dimension;
    }

    public OptimumType OptimumType { get; }
    public DimensionFilter Dimension { get; }
}