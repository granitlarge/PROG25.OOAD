using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public record EventMetricChangedComparedToThresholdTimestamp : Timestamp
{
    public EventMetricChangedComparedToThresholdTimestamp(Metric metric, decimal threshold, ComparisonResult comparisonResult)
        : base(TimestampType.NextMetricChange)
    {

        if (!metric.IsValidMetricValue(threshold))
        {
            throw new ArgumentOutOfRangeException(nameof(threshold), $"Threshold value {threshold} is not valid for metric {metric.Type}.");
        }

        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        ComparisonType = comparisonResult;
        Metric = metric;
        Threshold = threshold;
    }

    public Metric Metric { get; }
    public ComparisonResult ComparisonType { get; }
    public decimal Threshold { get; }

    public override bool HasOccured(EventStatistics currentMatchState)
    {
        var metricValue = currentMatchState.ExtractEventScope().Extract(Metric.Type);
        return IsExceeded(metricValue);
    }

    public override bool HasOccured(EventStatistics _, EventStatistics currentMatchState)
    {
        return HasOccured(currentMatchState);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = Metric.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ComparisonType == result;
    }
}