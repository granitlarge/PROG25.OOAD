using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record ScopedEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public ScopedEventMetricMarketConfiguration(ScopedMetricDefinition scopedMetricDefinition, EventDataTimestamp timestamp, string name)
        : base(scopedMetricDefinition.Metric, timestamp, name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Market name cannot be null or empty.", nameof(name));
        }
        ScopedMetricDefinition = scopedMetricDefinition;
    }

    public ScopedMetricDefinition ScopedMetricDefinition {get;}
}