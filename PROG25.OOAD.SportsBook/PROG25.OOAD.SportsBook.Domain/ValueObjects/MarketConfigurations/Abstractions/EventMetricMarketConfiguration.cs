using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

public abstract record EventMetricMarketConfiguration : MarketConfiguration
{
    public EventMetricMarketConfiguration
    (
        Metric metric,
        EventDataTimestamp timestamp,
        string name
    ) : base(timestamp, name)
    {
        Metric = metric;
    }

    public Metric Metric { get; }
}