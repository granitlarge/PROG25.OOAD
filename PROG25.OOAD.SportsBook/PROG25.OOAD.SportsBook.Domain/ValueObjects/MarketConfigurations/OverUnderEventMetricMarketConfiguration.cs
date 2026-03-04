using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record OverUnderEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OverUnderEventMetricMarketConfiguration
    (
        Metric metric,
        Scope scope,
        EventDataTimestamp timestamp,
        string name,
        decimal threshold
    ) : base(metric, scope, timestamp, name)
    {
        if (!metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentException("Invalid threshold value for the given metric.", nameof(threshold));
        }

        Threshold = threshold;
    }

    public decimal Threshold { get; }
}
