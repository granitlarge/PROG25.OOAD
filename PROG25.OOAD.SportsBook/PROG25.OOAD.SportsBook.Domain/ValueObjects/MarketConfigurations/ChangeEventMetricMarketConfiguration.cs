using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record ChangeScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ChangeScopedEventMetricMarketConfiguration
    (
        decimal referenceValue,
        ScopedMetricDefinition scopedMetricDefinition,
        EventDataTimestamp timestamp,
        ComparisonResult changeType,
        string name
    ) : base(scopedMetricDefinition, timestamp, name)
    {
        if (!scopedMetricDefinition.Metric.IsValidMetricValue(referenceValue))
        {
            throw new ArgumentException("Reference value is not valid for the metric.", nameof(referenceValue));
        }

        ReferenceValue = referenceValue;
        ChangeType = changeType;
    }

    public decimal ReferenceValue { get; }
    public ComparisonResult ChangeType { get; }
}