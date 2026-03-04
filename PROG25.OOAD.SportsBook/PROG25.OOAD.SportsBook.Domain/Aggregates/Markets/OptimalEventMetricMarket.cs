using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is the optimal value (e.g. maximum or minimum) among all scopes of a certain type at the time of settlement.
/// For example, this could be used to create a market that settles YES if a team's total goals at the end of the match are higher than both the other teams' total goals, and NO otherwise.
/// </summary>
public class OptimalEventMetricMarket : ScopedEventMetricMarket
{
    internal OptimalEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        ISet<(TeamId, PlayerId)> eventTeamPlayerPairs,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome,
        OptimalEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, eventTeamPlayerPairs, new HashSet<Outcome> { yesOutcome, noOutcome }, configuration)
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
    public override OptimalEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus Settle(EventData eventData)
    {
        var settlementAttemptStatus = base.Settle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus;
        }

        var winnerScopedMatchState = Configuration.Scope.ExtractScopedMetrics(eventData.Metrics);
        var allScopedMatchStatesExceptWinner = eventData.Metrics.ExtractAllScopes(Configuration.Scope.Type)
        .Where(scope => scope.Id != winnerScopedMatchState.Id);

        var winnerMetricValue = winnerScopedMatchState.Extract(Configuration.Metric.Type);
        var otherMetricValues = allScopedMatchStatesExceptWinner.Select(scope => scope.Extract(Configuration.Metric.Type));

        var isOptimum = Configuration.OptimumType switch
        {
            OptimumType.Maximum => winnerMetricValue > otherMetricValues.Max(),
            OptimumType.Minimum => winnerMetricValue < otherMetricValues.Min(),
            _ => throw new NotSupportedException($"Optimum type {Configuration.OptimumType} is not supported."),
        };

        var winningOutComeId = isOptimum ? YesOutcome.Id : NoOutcome.Id;
        Settle(winningOutComeId);
        return SettlementAttemptStatus.Completed;
    }

}