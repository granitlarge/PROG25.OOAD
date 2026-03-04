using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record OptimalScopedEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OptimalScopedEventMetricMarketConfiguration
    (
        ScopedMetricDefinition metric,
        EventDataTimestamp timestamp,
        string name,
        OptimumType optimumType
    ) : base(metric, timestamp, name)
    {
        OptimumType = optimumType;
        if (metric.Scope.Type == ScopeType.Event)
        {
            throw new ArgumentException("Scope type cannot be 'Event' for an optimal scoped metric market configuration.", nameof(metric));
        }
    }

    public OptimumType OptimumType { get; }
}
