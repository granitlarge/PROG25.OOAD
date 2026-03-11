using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record ScopedEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public ScopedEventMetricMarketConfiguration
    (
        Scope scope,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name
    ) : base(metric, timestamp, name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Market name cannot be null or empty.", nameof(name));
        }
        Scope = scope;
    }

    public Scope Scope { get; }
}