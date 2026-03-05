using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class EventType
{
    private readonly ImmutableHashSet<(ScopeType, MetricDefinition)> _supportedMetrics;
    private readonly ImmutableHashSet<Period> _periods;

    public EventType
    (
        EventTypeEnum eventType,
        HashSet<(ScopeType ScopeType, MetricDefinition MetricDefinition)> supportedMetrics,
        (ScopeType ScopeType, MetricDefinition MetricDefinition) metricThatDeterminesWinner,
        OptimumType optimumTypeThatDeterminesWinner,
        ISet<Period> periods
    )
    {
        if (!supportedMetrics.Contains(metricThatDeterminesWinner))
        {
            throw new ArgumentException("The supported metric scopes must include the metric and scope that determine the winner.");
        }

        if (metricThatDeterminesWinner.ScopeType == ScopeType.Event)
        {
            throw new ArgumentException("The metric that determines the winner cannot be scoped to the entire event .");
        }

        _supportedMetrics = [.. supportedMetrics];
        _periods = [.. periods];

        Id = new EventTypeId();
        Type = eventType;
        MetricThatDeterminesWinner = metricThatDeterminesWinner;
        OptimumTypeThatDeterminesWinner = optimumTypeThatDeterminesWinner;
    }

    public EventTypeId Id { get; }

    public EventTypeEnum Type { get; }

    public ImmutableHashSet<(ScopeType ScopeType, MetricDefinition MetricDefinition)> SupportedMetrics => _supportedMetrics;

    public (ScopeType ScopeType, MetricDefinition MetricDefinition) MetricThatDeterminesWinner { get; }

    /// <summary>
    /// This indicates whether a higher or lower value of the metric is better for determining the winner. 
    /// For example, in most sports, a higher number of points is better (OptimumType.Maximum). 
    /// However, in some cases, a lower time is better (OptimumType.Minimum), such as in a race.
    /// </summary>
    public OptimumType OptimumTypeThatDeterminesWinner { get; }

    public ImmutableHashSet<Period> Periods => _periods;
}