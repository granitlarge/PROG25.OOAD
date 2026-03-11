using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record OptimalScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OptimalScopedEventMetricMarketConfiguration
    (
        Scope scope,
        MetricDefinition metric,
        EventDataTimestamp timestamp,
        string name,
        OptimumType optimumType
    ) : base(scope, metric, timestamp, name)
    {
        OptimumType = optimumType;
        if (Scope.Type == ScopeType.Event)
        {
            throw new ArgumentException("Scope type cannot be 'Event' for an optimal scoped metric market configuration.", nameof(metric));
        }
    }

    public OptimumType OptimumType { get; }
}
