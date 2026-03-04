using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record ComparisonScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ComparisonScopedEventMetricMarketConfiguration
    (
        decimal referenceValue,
        Metric metric,
        Scope scope,
        EventDataTimestamp timestamp,
        ComparisonResult changeType,
        string name
    ) : base(metric, scope, timestamp, name)
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