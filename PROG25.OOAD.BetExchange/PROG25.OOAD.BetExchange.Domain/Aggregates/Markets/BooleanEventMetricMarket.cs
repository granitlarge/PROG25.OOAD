using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

public class BooleanEventMetricMarket : EventMetricMarket
{
    internal BooleanEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        BooleanEventMetricMarketConfiguration configuration
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

    public override BooleanEventMetricMarketConfiguration Configuration { get; }
    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var value = Configuration.Expression.Evaluate(eventData.Metrics);
        var isYes = value == Configuration.ExpectedExpressionResult;

        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);

        return SettlementAttemptStatus.Completed;
    }
}