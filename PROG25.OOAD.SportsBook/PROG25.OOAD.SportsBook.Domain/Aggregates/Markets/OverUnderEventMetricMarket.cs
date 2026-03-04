using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;

/// <summary>
/// Market that settles based on whether a specified scoped metric is over, under, or exactly equal to a specified threshold value at the time of settlement.
/// For example, this could be used to create a market that settles OVER if a team's total goals at the end of the match are higher than a specified value, UNDER if they are lower than that value, and PUSH if they are exactly equal to that value.
/// </summary>
public class OverUnderEventMetricMarket : ScopedEventMetricMarket
{
    internal OverUnderEventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        ISet<(TeamId, PlayerId)> teamPlayerPairs,
        OverUnderOutcome overOutcome,
        OverUnderOutcome underOutcome,
        OverUnderOutcome pushOutcome,
        OverUnderEventMetricMarketConfiguration configuration
    ) : base(eventId, eventData, teamPlayerPairs, new HashSet<Outcome> { overOutcome, underOutcome, pushOutcome }, configuration)
    {
        if (overOutcome.Type != OverUnderOutcomeType.Over)
        {
            throw new ArgumentException("overOutcome must have Type set to Over.", nameof(overOutcome));
        }
        if (underOutcome.Type != OverUnderOutcomeType.Under)
        {
            throw new ArgumentException("underOutcome must have Type set to Under.", nameof(underOutcome));
        }
        if (pushOutcome.Type != OverUnderOutcomeType.Push)
        {
            throw new ArgumentException("pushOutcome must have Type set to Push.", nameof(pushOutcome));
        }

        OverOutcome = overOutcome;
        UnderOutcome = underOutcome;
        PushOutcome = pushOutcome;
        Configuration = configuration;
    }

    public OverUnderOutcome OverOutcome { get; }
    public OverUnderOutcome UnderOutcome { get; }
    public OverUnderOutcome PushOutcome { get; }

    public override OverUnderEventMetricMarketConfiguration Configuration { get; }

    public override SettlementAttemptStatus Settle(EventData eventData)
    {
        var settlementAttemptStatus = base.Settle(eventData);
        if (settlementAttemptStatus != SettlementAttemptStatus.Possible)
        {
            return settlementAttemptStatus; // if the base settle method changed the status, we should not proceed with settling this bet
        }

        var scopeState = Configuration.Scope.ExtractScopedMetrics(eventData.Metrics);
        var actualMetricValue = scopeState.Extract(Configuration.Metric.Type);
        var compareResult = Configuration.Metric.Compare(actualMetricValue, Configuration.Threshold);

        var winningOutcomeId = compareResult switch
        {
            ComparisonResult.GreaterThan => OverOutcome.Id,
            ComparisonResult.LessThan => UnderOutcome.Id,
            ComparisonResult.Equal => PushOutcome.Id,
            _ => throw new NotImplementedException()
        };

        Settle(winningOutcomeId);

        return SettlementAttemptStatus.Completed;
    }
}