using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

public abstract record EventStatistics
{
    private readonly ImmutableHashSet<Metric> _supportedMetrics = new HashSet<Metric>
    {
        ReferenceValueBasedMetric.ElapsedActualTimeSeconds,
        ReferenceValueBasedMetric.ElapsedMatchTimeSeconds
    }.ToImmutableHashSet();

    public EventStatistics
    (
        EventId eventId,
        TimeSpan elapsedMatchTime,
        TimeSpan elapsedActualTime,
        EventStatus status
    )
    {
        ElapsedMatchTime = elapsedMatchTime;
        ElapsedActualTime = elapsedActualTime;
        Status = status;
        EventId = eventId;
    }

    public EventId EventId { get; }
    public EventStatus Status { get; private set; }
    public TimeSpan ElapsedMatchTime { get; private set; }
    public TimeSpan ElapsedActualTime { get; private set; }

    internal virtual ScopedEventStatistics ExtractTeamScope(TeamId teamId) { throw new NotImplementedException(); }
    internal virtual ScopedEventStatistics ExtractPlayerScope(PlayerId playerId) { throw new NotImplementedException(); }
    internal abstract ScopedEventStatistics ExtractEventScope();
    internal abstract List<ScopedEventStatistics> ExtractAllScopes(ScopeType scopeType);

    public virtual void UpdateMetric(TeamId teamId, PlayerId playerId, MetricType metricType, decimal newValue)
    {
        switch (metricType)
        {
            case MetricType.ElapsedMatchTimeSeconds:
                ElapsedMatchTime = TimeSpan.FromSeconds(Math.Floor((double)newValue));
                break;
            case MetricType.ElapsedActualTimeSeconds:
                ElapsedActualTime = TimeSpan.FromSeconds(Math.Floor((double)newValue));
                break;
            default:
                throw new NotSupportedException($"Metric type {metricType} is not supported for update in EventState.");
        }
    }

    protected abstract ImmutableHashSet<Metric> SupportedMetrics { get; }

    internal ImmutableHashSet<Metric> GetSupportedMetrics()
    {
        return _supportedMetrics.Union(SupportedMetrics);
    }

    public bool IsSupportedMetric(MetricType metricType)
    {
        return GetSupportedMetrics().Any(m => m.Type == metricType);
    }
}