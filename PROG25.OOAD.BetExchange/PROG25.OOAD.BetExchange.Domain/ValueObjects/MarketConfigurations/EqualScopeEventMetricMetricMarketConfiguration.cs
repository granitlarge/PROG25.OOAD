using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record EqualScopeEventMetricMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public EqualScopeEventMetricMetricMarketConfiguration
    (
        ScopeType scopeType,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name
    )
        : base(metric, timestamp, name)
    {
        if (scopeType == ScopeType.Event)
        {
            throw new ArgumentException("Scope type cannot be 'Event' for an equal scoped metric market configuration.", nameof(scopeType));
        }

        ScopeType = scopeType;
    }

    public ScopeType ScopeType { get; }
}