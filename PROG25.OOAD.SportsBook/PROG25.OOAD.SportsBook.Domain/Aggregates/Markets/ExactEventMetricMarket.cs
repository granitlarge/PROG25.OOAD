using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is equal to an expected value at the time of settlement.
/// For example, this could be used to create a market that settles YES if a team's total goals at the end of the match are equal to a specified value, and NO otherwise.
/// </summary>
public static class ExactEventMetricMarket
{
    public static ComparisonScopedEventMetricMarket Create
    (
        EventId eventId,
        EventData eventData,
        ISet<(TeamId, PlayerId)> teamPlayerPairs,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ExactEventMetricMarketConfiguration configuration
    )
    {
        var config = new ComparisonScopedEventMetricMarketConfiguration
        (configuration.ExpectedValue, configuration.Metric, configuration.Scope, configuration.Timestamp, ComparisonResult.Equal, configuration.Name);
        return new ComparisonScopedEventMetricMarket(eventId, eventData, teamPlayerPairs, yesOutcome, noOutcome, config);
    }
}