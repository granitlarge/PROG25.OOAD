using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class EventType
{
    public EventType
    (
        ValueObjects.EventType eventType,
        Metric metricThatDeterminesWinner,
        ScopeType scopeTypeThatDeterminesWinner,
        OptimumType optimumTypeThatDeterminesWinner,
        ISet<(MetricType, ScopeType)> supportedMetricScopes
    )
    {
        Id = new EventTypeId();
        Type = eventType;
        MetricThatDeterminesWinner = metricThatDeterminesWinner;
        ScopeTypeThatDeterminesWinner = scopeTypeThatDeterminesWinner;
        OptimumTypeThatDeterminesWinner = optimumTypeThatDeterminesWinner;

        if (!supportedMetricScopes.Contains((metricThatDeterminesWinner.Type, scopeTypeThatDeterminesWinner)))
        {
            throw new ArgumentException("The supported metric scopes must include the metric and scope that determine the winner.");
        }

        SupportedMetricScopes = supportedMetricScopes;
    }

    public EventTypeId Id { get; }

    public ValueObjects.EventType Type { get; }

    public ISet<(MetricType, ScopeType)> SupportedMetricScopes { get; }

    /// <summary>
    /// This is the metric that determines the winner of the event. 
    /// For example, in a soccer match, the metric that determines the winner is typically "Points" (i.e., goals scored). 
    /// In a basketball game, it would also be "Points". 
    /// In a tennis match, it might be "Games Won". 
    /// This metric is crucial for determining the outcome of bets placed on the event.
    /// </summary>
    public Metric MetricThatDeterminesWinner { get; }

    // <summary>
    /// This indicates the scope of the metric that determines the winner. 
    /// For example, in a soccer match, the scope is "Team" (i.e., the metric is evaluated for each team).
    /// </summary>
    public ScopeType ScopeTypeThatDeterminesWinner { get; }

    /// <summary>
    /// This indicates whether a higher or lower value of the metric is better for determining the winner. 
    /// For example, in most sports, a higher number of points is better (OptimumType.Maximum). 
    /// However, in some cases, a lower time is better (OptimumType.Minimum), such as in a race.
    /// </summary>
    public OptimumType OptimumTypeThatDeterminesWinner { get; }
}