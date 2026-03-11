using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Examples;

public record ExactScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ExactScopedEventMetricMarketConfiguration
    (
                Scope scope,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name,
        decimal expectedValue
    ) : base(scope, metric, timestamp, name)
    {
        if (!metric.IsValidMetricValue(expectedValue))
        {
            throw new ArgumentException("Invalid expected value for the given metric.", nameof(expectedValue));
        }
        ExpectedValue = expectedValue;
    }

    public decimal ExpectedValue { get; }
}