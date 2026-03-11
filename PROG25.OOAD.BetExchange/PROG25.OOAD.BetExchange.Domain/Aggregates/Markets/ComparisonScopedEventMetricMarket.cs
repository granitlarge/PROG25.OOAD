using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

public class ComparisonScopedEventMetricMarket : ScopedEventMetricMarket
{
    internal ComparisonScopedEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        ComparisonScopedEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, configuration, yesOutcome, noOutcome)
    {
        Configuration = configuration;
    }

    public override ComparisonScopedEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var metricValue = eventData.Metrics.Extract(Configuration.Scope, Configuration.Metric);
        var compareResult = Configuration.Metric.Compare(Configuration.ReferenceValue, metricValue.Value);
        var isYes = compareResult == Configuration.ExpectedComparisonResult;

        Settle(isYes ? YesOutcome.Id : NoOutcome.Id);

        return SettlementAttemptStatus.Completed;
    }
}