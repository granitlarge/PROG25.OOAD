using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is the optimal value (e.g. maximum or minimum) among all scopes of a certain type at the time of settlement.
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
    ) : base(eventId, eventData, configuration, new HashSet<Outcome> { yesOutcome, noOutcome })
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
        Configuration = configuration;
    }

    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }
    public override OptimalScopedEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus Settle(EventData eventData)
    {
        var settlementAttemptStatus = base.Settle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var specificMetric = eventData.Metrics.Extract(Configuration.ScopedMetricDefinition);
        var allMetricInScopeExceptSpecificMetric = eventData.Metrics
        .ExtractAll(Configuration.ScopedMetricDefinition.Scope.Type, Configuration.ScopedMetricDefinition.Metric)
        .Where(mv => mv != specificMetric);

        var isOptimal = Configuration.OptimumType switch
        {
            OptimumType.Maximum => allMetricInScopeExceptSpecificMetric.Max()!.Value < specificMetric.Value,
            OptimumType.Minimum => allMetricInScopeExceptSpecificMetric.Min()!.Value > specificMetric.Value,
            _ => throw new InvalidOperationException("Unsupported optimality type.")
        };

        Settle(isOptimal ? YesOutcome.Id : NoOutcome.Id);
        return SettlementAttemptStatus.Completed;
    }

}