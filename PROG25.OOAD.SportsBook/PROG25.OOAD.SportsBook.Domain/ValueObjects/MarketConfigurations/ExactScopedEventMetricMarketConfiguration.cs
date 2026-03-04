using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record ExactScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ExactScopedEventMetricMarketConfiguration
    (
        ScopedMetricDefinition scopedMetricDefinition,
        EventDataTimestamp timestamp,
        string name,
        decimal expectedValue
    ) : base(scopedMetricDefinition, timestamp, name)
    {
        if (!scopedMetricDefinition.Metric.IsValidMetricValue(expectedValue))
        {
            throw new ArgumentException("Invalid expected value for the given metric.", nameof(expectedValue));
        }
        ExpectedValue = expectedValue;
    }

    public decimal ExpectedValue { get; }
}