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
public class EqualityEventMetricMarket : EventMetricMarket
{
    internal EqualityEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        EqualityEventMetricMarketConfiguration marketConfiguration
    )
        : base(eventId, eventData, marketConfiguration, new HashSet<Outcome> { yesOutcome, noOutcome })
    {
        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("The yesOutcome must be a YES outcome.", nameof(yesOutcome));
        }

        if (noOutcome.IsYes)
        {
            throw new ArgumentException("The noOutcome must be a NO outcome.", nameof(noOutcome));
        }

        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
        Configuration = marketConfiguration;
    }

    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override EqualityEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var win = eventData.Metrics.ExtractAll(Configuration.DimensionNames, Configuration.Metric).Select(e => e.Value).Distinct().Count() == 1;
        Settle(win ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}