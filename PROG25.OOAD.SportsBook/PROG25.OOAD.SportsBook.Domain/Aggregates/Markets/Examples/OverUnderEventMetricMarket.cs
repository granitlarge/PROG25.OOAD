using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Examples;

/// <summary>
/// Market that settles based on whether a specified scoped metric is over, under, or exactly equal to a specified threshold value at the time of settlement.
/// For example, this could be used to create a market that settles OVER if a team's total goals at the end of the match are higher than a specified value, UNDER if they are lower than that value, and PUSH if they are exactly equal to that value.
/// </summary>
public class OverUnderEventMetricMarket
{
    internal static (ComparisonScopedEventMetricMarket Over, ComparisonScopedEventMetricMarket Under) Create
    (
        EventId eventId,
        EventData eventData,
        OverUnderOutcome overOutcome,
        OverUnderOutcome underOutcome,
        OverUnderOutcome pushOutcome,
        OverUnderScopedEventMetricMarketConfiguration configuration
    )
    {
        var overMarket = new ComparisonScopedEventMetricMarket
        (
            eventId,
            eventData,
            new YesNoOutcome(overOutcome.Odds, true),
#warning This is a simplification - in a real implementation, we would likely want to calculate the push odds in a more sophisticated way to ensure that the market is balanced and does not allow for arbitrage opportunities. 
#warning For example, we might want to set the push odds such that the expected value of a bet on over or under is the same, taking into account the probabilities of each outcome and the odds offered.
            new YesNoOutcome(pushOutcome.Odds < underOutcome.Odds ? pushOutcome.Odds : underOutcome.Odds, false),
            new ComparisonScopedEventMetricMarketConfiguration
            (
                configuration.Threshold,
                configuration.Scope,
                configuration.Metric,
                configuration.Timestamp,
                ComparisonResult.GreaterThan,
                "Over Market"
            )
        );

        var underMarket = new ComparisonScopedEventMetricMarket
        (
            eventId,
            eventData,
            new YesNoOutcome(underOutcome.Odds, true),
            new YesNoOutcome(pushOutcome.Odds < overOutcome.Odds ? pushOutcome.Odds : overOutcome.Odds, false),
            new ComparisonScopedEventMetricMarketConfiguration
            (
                configuration.Threshold,
                configuration.Scope,
                configuration.Metric,
                configuration.Timestamp,
                ComparisonResult.LessThan,
                "Under Market"
            )
        );

        return (overMarket, underMarket);
    }
}