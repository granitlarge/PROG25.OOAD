using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Examples;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Examples;

/// <summary>
/// Market that settles based on whether a specified scoped metric has changed (or not changed) in a certain way compared to a reference value at the time of settlement.
/// </summary>
public static class ChangeScopedEventMetricMarket
{
    public static ComparisonScopedEventMetricMarket Create
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ChangeScopedEventMetricMarketConfiguration configuration
    )
    {
        var config = new ComparisonScopedEventMetricMarketConfiguration
        (configuration.ReferenceValue, configuration.Scope, configuration.Metric, configuration.Timestamp, configuration.ChangeType, configuration.Name);
        return new ComparisonScopedEventMetricMarket(eventId, eventData, yesOutcome, noOutcome, config);
    }
}