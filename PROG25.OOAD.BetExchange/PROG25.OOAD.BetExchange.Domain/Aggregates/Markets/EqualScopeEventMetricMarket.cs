using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

/// <summary>
/// A market that settles based on whether a specified metric has the same value across all scopes of a certain type at the time of settlement. 
/// For example, this could be used to create a market that settles YES if all teams have the same number of goals at the end of the match, and NO otherwise.
/// I.e., it can be used to implement draws.
/// </summary> 
public class EqualScopeEventMetricMarket : EventMetricMarket
{
    internal EqualScopeEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        EqualScopeEventMetricMetricMarketConfiguration marketConfiguration
    )
        : base(eventId, eventData, marketConfiguration, yesOutcome, noOutcome)
    {
        Configuration = marketConfiguration;
    }

    public override EqualScopeEventMetricMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }
        var win = eventData.Metrics.ExtractAll(Configuration.ScopeType, Configuration.Metric).Distinct().Count() == 1;
        Settle(win ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}