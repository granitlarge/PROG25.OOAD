using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToThresholdTimestamp : EventDataTimestamp
{
    public EventMetricChangedComparedToThresholdTimestamp
    (
        ScopedMetricDefinition scopedMetricDefinition,
        decimal threshold,
        ComparisonResult comparisonResult
    )
        : base(EventDataTimestampType.EventData)
    {

        if (!scopedMetricDefinition.Metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), $"Threshold value {threshold} is not valid for metric {scopedMetricDefinition.Metric.Name}.");
        }

        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        ExpectedComparisonResult = comparisonResult;
        ScopedMetricDefinition = scopedMetricDefinition;
        Threshold = threshold;
    }

    public ScopedMetricDefinition ScopedMetricDefinition { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public decimal Threshold { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var metricValue = currentEventData.Metrics.Extract(ScopedMetricDefinition);
        return IsExceeded(metricValue.Value);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = ScopedMetricDefinition.Metric.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ExpectedComparisonResult == result;
    }
}