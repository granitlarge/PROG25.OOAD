using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToThresholdTimestamp : EventDataTimestamp
{
    public EventMetricChangedComparedToThresholdTimestamp
    (
        Scope scope,
        MetricDefinition metric,
        decimal threshold,
        ComparisonResult comparisonResult
    )
        : base(EventDataTimestampType.EventData)
    {
        if (!metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), $"Threshold value {threshold} is not valid for metric {metric.Name}.");
        }

        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        Scope = scope;
        Metric = metric;
        ExpectedComparisonResult = comparisonResult;
        Threshold = threshold;
    }

    public Scope Scope { get; }
    public MetricDefinition Metric { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public decimal Threshold { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var metricValue = currentEventData.Metrics.Extract(Scope, Metric);
        return IsExceeded(metricValue.Value);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = Metric.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ExpectedComparisonResult == result;
    }
}