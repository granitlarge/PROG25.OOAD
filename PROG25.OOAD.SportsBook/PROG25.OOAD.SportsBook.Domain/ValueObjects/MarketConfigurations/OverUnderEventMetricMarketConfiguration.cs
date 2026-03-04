using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

public record OverUnderEventMetricMarketConfiguration : ScopedEventMetricMarketConfiguration
{
    public OverUnderEventMetricMarketConfiguration
    (
        Metric metric,
        Scope scope,
        Timestamp timestamp,
        string name,
        ReferenceValueDecimalComparer threshold
    ) : base(metric, scope, timestamp, name)
    {
        if (!metric.IsValidMetricValue(threshold.ReferenceValue))
        {
            throw new ArgumentException("Invalid threshold value for the given metric.", nameof(threshold));
        }

        Threshold = threshold;
    }

    public ReferenceValueDecimalComparer Threshold { get; }
}
