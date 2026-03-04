using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record ScopedEventMetricMarketConfiguration : EventMetricMarketConfiguration
{
    public ScopedEventMetricMarketConfiguration
    (
        Metric metric,
        Scope scope,
        Timestamp timestamp,
        string name
    ) : base(metric, timestamp, name)
    {
        Scope = scope;
    }

    public Scope Scope { get; }
}