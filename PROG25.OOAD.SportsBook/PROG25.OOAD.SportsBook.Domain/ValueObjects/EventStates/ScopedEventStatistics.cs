using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

public abstract record ScopedEventStatistics
{
    protected ScopedEventStatistics(ScopedEventStateId id)
    {
        Id = id;
    }

    public ScopedEventStateId Id { get; }
    public abstract decimal Extract(MetricType metricType);
}
