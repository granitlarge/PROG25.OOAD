using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToReferenceValueTimestamp : EventDataTimestamp
{
    public EventMetricChangedComparedToReferenceValueTimestamp
    (
        ImmutableHashSet<DimensionFilter> dimensions,
        MetricDefinition metric,
        decimal referenceValue,
        ComparisonResult comparisonResult
    )
        : base(EventDataTimestampType.EventData)
    {
        if (!metric.IsValidMetricValue(referenceValue))
        {
            throw new ArgumentOutOfRangeException(nameof(referenceValue), $"Reference value {referenceValue} is not valid for metric {metric.Name}.");
        }

        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        Dimensions = dimensions;
        Metric = metric;
        ExpectedComparisonResult = comparisonResult;
        Threshold = referenceValue;
    }

    public MetricDefinition Metric { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public ImmutableHashSet<DimensionFilter> Dimensions { get; }
    public decimal Threshold { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var value = currentEventData.Metrics.Extract(Dimensions, Metric);
        return IsExceeded(value);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = Metric.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ExpectedComparisonResult == result;
    }
}