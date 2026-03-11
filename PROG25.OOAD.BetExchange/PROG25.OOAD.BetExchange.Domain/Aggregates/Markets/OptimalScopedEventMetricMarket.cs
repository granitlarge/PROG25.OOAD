using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is the optimal value (e.g. maximum or minimum) among all scopes of the same type at the time of settlement.
/// For example, this could be used to create a market that settles YES if a team's total goals at the end of the match are higher than both the other teams' total goals, and NO otherwise.
/// I.e. it can be used to implement wins or losses.
/// </summary>
public class OptimalScopedEventMetricMarket : ScopedEventMetricMarket
{
    internal OptimalScopedEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        OptimalScopedEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, configuration, yesOutcome, noOutcome)
    {
        Configuration = configuration;
    }

    public override OptimalScopedEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus TrySettle(EventData eventData)
    {
        var settlementAttemptStatus = base.TrySettle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var specificMetric = eventData.Metrics.Extract(Configuration.Scope, Configuration.Metric);
        var allMetricInScopeExceptSpecificMetric = eventData.Metrics
        .ExtractAll(Configuration.Scope.Type, Configuration.Metric)
        .Where(mv => mv != specificMetric);

        var isOptimal = Configuration.OptimumType switch
        {
            OptimumType.Maximum => allMetricInScopeExceptSpecificMetric.Max(e => e.Value) < specificMetric.Value,
            OptimumType.Minimum => allMetricInScopeExceptSpecificMetric.Min(e => e.Value) > specificMetric.Value,
            _ => throw new InvalidOperationException("Unsupported optimality type.")
        };

        Settle(isOptimal ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }
}