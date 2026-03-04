using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record EqualScopedEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public EqualScopedEventMetricMarketConfiguration
    (
        Metric metric, 
        ScopeType scope, 
        EventDataTimestamp timestamp, 
        string name
    )
        : base(metric, timestamp, name)
    {
        if (scope == ScopeType.Event)
        {
            throw new ArgumentException("ScopeType cannot be Match for EqualScopedEventMetricMarketConfiguration.", nameof(scope));
        }
        ScopeType = scope;
    }

    public ScopeType ScopeType { get; }
}