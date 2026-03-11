using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

public class ComparisonEventMetricMarket : EventMetricMarket
{
    internal ComparisonEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ComparisonScopedEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, configuration, new HashSet<Outcome> { yesOutcome, noOutcome })
    {
        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("Yes outcome must be a yes outcome.", nameof(yesOutcome));
        }

        if (noOutcome.IsYes)
        {
            throw new ArgumentException("No outcome must be a no outcome.", nameof(noOutcome));
        }

        Configuration = configuration;
        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
    }

    public override ComparisonScopedEventMetricMarketConfiguration Configuration { get; }
    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var value = eventData.Metrics.Extract(Configuration.Dimensions, Configuration.Metric);
        var compareResult = Configuration.Metric.Compare(Configuration.ReferenceValue, value);
        var isYes = compareResult == Configuration.ExpectedComparisonResult;

        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);

        return SettlementAttemptStatus.Completed;
    }
}