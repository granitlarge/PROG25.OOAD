using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record ComparisonScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public ComparisonScopedEventMetricMarketConfiguration
    (
        decimal referenceValue,
        ScopedMetricDefinition scopedMetricDefinition,
        EventDataTimestamp timestamp,
        ComparisonResult expectedComparisonResult,
        string name
    ) : base(scopedMetricDefinition, timestamp, name)
    {
        if (!scopedMetricDefinition.Metric.IsValidMetricValue(referenceValue))
        {
            throw new ArgumentException("Reference value is not valid for the metric.", nameof(referenceValue));
        }

        ReferenceValue = referenceValue;
        ExpectedComparisonResult = expectedComparisonResult;
    }

    public decimal ReferenceValue { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
}