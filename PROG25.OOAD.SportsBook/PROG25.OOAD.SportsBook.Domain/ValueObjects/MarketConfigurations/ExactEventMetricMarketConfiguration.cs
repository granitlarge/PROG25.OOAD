using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record ExactEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ExactEventMetricMarketConfiguration
    (
        Metric metric,
        Scope scope,
        EventDataTimestamp timestamp,
        string name,
        decimal expectedValue
    ) : base(metric, scope, timestamp, name)
    {
        if (!metric.IsValidMetricValue(expectedValue))
        {
            throw new ArgumentException("Invalid expected value for the given metric.", nameof(expectedValue));
        }
        ExpectedValue = expectedValue;
    }

    public decimal ExpectedValue { get; }
}