using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record OverUnderScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OverUnderScopedEventMetricMarketConfiguration
    (
        ScopedMetricDefinition scopedMetricDefinition,
        EventDataTimestamp timestamp,
        string name,
        decimal threshold
    ) : base(scopedMetricDefinition, timestamp, name)
    {
        if (!scopedMetricDefinition.Metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentException("Invalid threshold value for the given metric.", nameof(threshold));
        }

        Threshold = threshold;
    }

    public decimal Threshold { get; }
}