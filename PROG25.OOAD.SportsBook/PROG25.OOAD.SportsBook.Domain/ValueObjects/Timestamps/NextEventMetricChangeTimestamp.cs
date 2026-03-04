using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

/// <summary>
/// A timestamp representing the next change of a specific metric in a specific direction (increase/decrease).
/// </summary>
public record NextEventMetricChangeTimestamp : Timestamp
{
    public NextEventMetricChangeTimestamp
    (
        Metric metric,
        ComparisonResult? changeType
    ) : base(TimestampType.NextMetricChange)
    {
        Metric = metric;
        ChangeType = changeType;
    }

    public Metric Metric { get; }
    public ComparisonResult? ChangeType { get; }

    public override bool HasOccured(EventStatistics previousMatchState, EventStatistics currentMatchState)
    {
        var currentMetricValue = currentMatchState.ExtractEventScope().Extract(Metric.Type);
        var previousMetricValue = previousMatchState.ExtractEventScope().Extract(Metric.Type);

        var comparisonResult = Metric.Compare(previousMetricValue, currentMetricValue);

        if (comparisonResult == ComparisonResult.Equal)
        {
            return false;
        }

        if (ChangeType == null)
        {
            return true;
        }

        return ChangeType == comparisonResult;
    }

    public override bool HasOccured(EventStatistics currentMatchState)
    {
        return false;
    }
}