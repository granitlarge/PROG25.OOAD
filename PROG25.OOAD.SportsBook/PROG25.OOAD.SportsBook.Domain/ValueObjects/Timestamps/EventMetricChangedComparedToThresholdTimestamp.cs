using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToThresholdTimestamp : EventDataTimestamp
{
    public EventMetricChangedComparedToThresholdTimestamp
    (
        Metric metric,
        Scope scope,
        decimal threshold,
        ComparisonResult comparisonResult
    )
        : base(EventDataTimestampType.EventData)
    {

        if (!metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), $"Threshold value {threshold} is not valid for metric {metric.Type}.");
        }

        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        ExpectedComparisonResult = comparisonResult;
        Metric = metric;
        Scope = scope;
        Threshold = threshold;
    }

    public Metric Metric { get; }
    public Scope Scope { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public decimal Threshold { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var metricValue = Scope.Type switch
        {
            ScopeType.Event => currentEventData.Metrics.ExtractEventScope().Extract(Metric.Type),
            ScopeType.Team => Scope.ExtractScopedMetrics(currentEventData.Metrics).Extract(Metric.Type),
            ScopeType.Player => Scope.ExtractScopedMetrics(currentEventData.Metrics).Extract(Metric.Type),
            _ => throw new NotImplementedException()
        };
        return IsExceeded(metricValue);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = Metric.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ExpectedComparisonResult == result;
    }
}