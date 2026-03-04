using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record EventMetricMarketConfiguration : MarketConfiguration
{
    public EventMetricMarketConfiguration
    (
        Metric metric,
        Timestamp timestamp,
        string name
    ) : base(name)
    {
        Metric = metric;
        Timestamp = timestamp;
    }

    public Metric Metric { get; }
    public Timestamp Timestamp { get; }
}