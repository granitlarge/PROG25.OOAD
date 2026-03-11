using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Examples;

public record OverUnderScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OverUnderScopedEventMetricMarketConfiguration
    (
                Scope scope,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name,
        decimal threshold
    ) : base(scope, metric, timestamp, name)
    {
        if (!metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentException("Invalid threshold value for the given metric.", nameof(threshold));
        }

        Threshold = threshold;
    }

    public decimal Threshold { get; }
}