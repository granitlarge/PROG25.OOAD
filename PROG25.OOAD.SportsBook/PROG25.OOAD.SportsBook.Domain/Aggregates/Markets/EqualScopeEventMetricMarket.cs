using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Values;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// A market that settles based on whether a specified metric has the same value across all scopes of a certain type at the time of settlement. 
/// For example, this could be used to create a market that settles YES if all teams have the same number of goals at the end of the match, and NO otherwise.
/// </summary>
public class EqualScopeEventMetricMarket : EventMetricMarket
{
    internal EqualScopeEventMetricMarket
    (
        EventId eventId,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        EqualScopeEventMetricMetricMarketConfiguration marketConfiguration
    )
        : base(eventId, marketConfiguration, new HashSet<Outcome> { yesOutcome, noOutcome })
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

    public override EqualScopeEventMetricMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus Settle(EventData eventData)
    {
        var settlementAttemptStatus = base.Settle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }
        var win = eventData.Metrics.ExtractAll(Configuration.ScopeType, Configuration.Metric).Distinct().Count() == 1;
        Settle(win ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}