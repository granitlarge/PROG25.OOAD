using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

/// <summary>
/// A market that settles based on whether all extracted metric values are equal for a specific metric and set of dimensions. 
/// The market configuration specifies the metric and dimensions to extract, and the market settles to YES if all extracted values are the same, and NO otherwise.
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

        var metricValues = eventData.Metrics.GetByDefinition(Configuration.MetricDefinition);
        var win = Configuration.MetricDefinition
                        .GroupBy(Configuration.DimensionNames, metricValues)
                        .Select(g => Configuration.MetricDefinition.Aggregate(g.Values))
                        .Distinct()
                        .Count() == 1;

        Settle(win ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}