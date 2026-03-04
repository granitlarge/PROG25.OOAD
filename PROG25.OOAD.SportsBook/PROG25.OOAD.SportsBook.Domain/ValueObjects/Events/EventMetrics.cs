using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

public abstract record EventMetrics
{
    private readonly ImmutableHashSet<Metric> _supportedMetrics = new HashSet<Metric>
    {
        ReferenceValueBasedMetric.ElapsedActualTimeSeconds,
        ReferenceValueBasedMetric.ElapsedMatchTimeSeconds
    }.ToImmutableHashSet();

    public EventMetrics
    (
        TimeSpan elapsedMatchTime,
        TimeSpan elapsedActualTime
    )
    {
        ElapsedMatchTime = elapsedMatchTime;
        ElapsedActualTime = elapsedActualTime;
    }

    public TimeSpan ElapsedMatchTime { get; private set; }
    public TimeSpan ElapsedActualTime { get; private set; }

    internal abstract ScopedEventMetrics ExtractScope(Scope scope);
    internal virtual ScopedEventMetrics ExtractTeamScope(TeamId teamId) { throw new NotImplementedException(); }
    internal virtual ScopedEventMetrics ExtractPlayerScope(PlayerId playerId) { throw new NotImplementedException(); }
    internal abstract ScopedEventMetrics ExtractEventScope();
    internal abstract List<ScopedEventMetrics> ExtractAllScopes(ScopeType scopeType);

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