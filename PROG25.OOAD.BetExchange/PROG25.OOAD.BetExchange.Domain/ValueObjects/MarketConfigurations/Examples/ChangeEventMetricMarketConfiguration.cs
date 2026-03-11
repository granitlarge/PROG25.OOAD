using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Examples;

public record ChangeScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ChangeScopedEventMetricMarketConfiguration
    (
        decimal referenceValue,
        Scope scope,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        ComparisonResult changeType,
        string name
    ) : base(scope, metric, timestamp, name)
    {
        if (!metric.IsValidMetricValue(referenceValue))
        {
            throw new ArgumentException("Reference value is not valid for the metric.", nameof(referenceValue));
        }

        ReferenceValue = referenceValue;
        ChangeType = changeType;
    }

    public decimal ReferenceValue { get; }
    public ComparisonResult ChangeType { get; }
}