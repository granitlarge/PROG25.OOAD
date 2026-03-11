using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record EventMetricMarketConfiguration
{
    public EventMetricMarketConfiguration
    (
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Market name cannot be null or empty.", nameof(name));
        }

        Metric = metric;
        Name = name.Trim();
        Timestamp = timestamp;
    }

    public MetricDefinition Metric { get; }
    public string Name { get; }
    public EventDataTimestamp Timestamp { get; }
}