using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record OptimalEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OptimalEventMetricMarketConfiguration
    (
        Metric metric,
        Scope scope,
        Timestamp timestamp,
        string name,
        OptimumType optimumType
    ) : base(metric, scope, timestamp, name)
    {
        OptimumType = optimumType;
    }

    public OptimumType OptimumType { get; }
}
