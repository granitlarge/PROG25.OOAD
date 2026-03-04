namespace PROG25.OOAD.Domain.ValueObjects.Metrics;

public abstract record Metric
{
    public abstract decimal Resolve(MatchState matchStatistics);
}