using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToReferenceValueTimestamp : EventDataTimestamp
{
    public EventMetricChangedComparedToReferenceValueTimestamp
    (
        Scope scope,
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

        Scope = scope;
        Metric = metric;
        ExpectedComparisonResult = comparisonResult;
        Threshold = referenceValue;
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