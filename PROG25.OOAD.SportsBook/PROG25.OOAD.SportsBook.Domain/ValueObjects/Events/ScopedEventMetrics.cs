using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

public abstract record ScopedEventMetrics
{
    protected ScopedEventMetrics(ScopedEventStateId id)
    {
        Id = id;
    }

    public ScopedEventStateId Id { get; }
    public abstract decimal Extract(MetricType metricType);
}
