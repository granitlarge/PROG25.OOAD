using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// A market that settles based on whether a specified metric has the same value across all scopes of a certain type at the time of settlement. 
/// For example, this could be used to create a market that settles YES if all teams have the same number of goals at the end of the match, and NO otherwise.
/// </summary>
public class EqualScopedEventMetricMarket : EventMetricMarket
{
    internal EqualScopedEventMetricMarket
    (
        Event @event,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        EqualScopedEventMetricMarketConfiguration marketConfiguration
    )
        : base(@event, marketConfiguration, new HashSet<Outcome> { yesOutcome, noOutcome })
    {
        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("yesOutcome must have IsYes set to true.", nameof(yesOutcome));
        }
        if (noOutcome.IsYes)
        {
            throw new ArgumentException("noOutcome must have IsYes set to false.", nameof(noOutcome));
        }

        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
        Configuration = marketConfiguration;
    }

    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override EqualScopedEventMetricMarketConfiguration Configuration { get; }

    public override MarketStatus Settle(EventStatistics _, Event @event)
    {
        var baseStatus = base.Settle(_, @event);
        if (!IsSettleable())
        {
            return Status; // if the base settle method changed the status, we should not proceed with settling this bet
        }

        var metricType = Configuration.Metric.Type;
        var scopeType = Configuration.ScopeType;

        bool win = @event.Statistics
                    .ExtractAllScopes(scopeType)
                    .Select(scopedMatchState => scopedMatchState.Extract(metricType))
                    .Distinct()
                    .Count() == 1;

        var isYes = win;
        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);
        return Status;
    }
}