using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record EqualityEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public EqualityEventMetricMarketConfiguration
    (
        MetricDefinition metric,
        List<string> dimensionNames,
        EventDataTimestamp timestamp,
        string name
    ) : base(metric, timestamp, name)
    {
        if (dimensionNames.Count == 0)
        {
            throw new ArgumentException("At least one dimension must be specified.", nameof(dimensionNames));
        }

        if (dimensionNames.Any(dn => !metric.Dimension.NameToTypeMapping.ContainsKey(dn)))
        {
            throw new ArgumentException("All dimension names must be valid for the metric definition.", nameof(dimensionNames));
        }

        DimensionNames = dimensionNames;
    }

    public List<string> DimensionNames { get; }
}