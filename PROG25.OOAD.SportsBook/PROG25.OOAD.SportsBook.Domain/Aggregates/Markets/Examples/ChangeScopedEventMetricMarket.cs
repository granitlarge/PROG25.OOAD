using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

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
        (configuration.ReferenceValue, configuration.ScopedMetricDefinition, configuration.Timestamp, configuration.ChangeType, configuration.Name);
        return new ComparisonScopedEventMetricMarket(eventId, eventData, yesOutcome, noOutcome, config);
    }
}