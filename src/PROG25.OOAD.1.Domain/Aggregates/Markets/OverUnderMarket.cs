using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.Domain.Aggregates.Markets;

public class OverUnderMarket : Market
{
    private readonly Metric _metric;

    protected OverUnderMarket(MarketId id,
        MatchId matchId,
        MatchEventType settlingEvent,
        List<OverUnderOutcome> outcomes,
        decimal threshold,
        Metric metric,
        OutcomeId? settledOutcomeId) : base(id, matchId, MarketType.OverUnder, [settlingEvent], outcomes.Cast<Outcome>().ToList(), settledOutcomeId)
    {
        var overOutcomesCount = outcomes.Count(o => o.Value == OverUnderOutcomeValue.Over);
        var underOutcomesCount = outcomes.Count(o => o.Value == OverUnderOutcomeValue.Under);
        var pushOutcomesCount = outcomes.Count(o => o.Value == OverUnderOutcomeValue.Push);
        if (overOutcomesCount != 1 || underOutcomesCount != 1 || pushOutcomesCount != 1)
        {
            throw new ArgumentException("Over/Under market must have exactly one 'over' outcome, one 'under' outcome, and one 'push' outcome.", nameof(outcomes));
        }

        Threshold = threshold;
        _metric = metric;
    }

    public decimal Threshold { get; }

    protected override OutcomeId? SettleInternal(MatchEvent _, MatchState matchStatistics)
    {
        var outcomes = Outcomes.Cast<OverUnderOutcome>().ToList();
        var metricValue = _metric.Resolve(matchStatistics);
        if (metricValue > Threshold)
        {
            return outcomes.First(o => o.Value == OverUnderOutcomeValue.Over).Id;
        }
        else if (metricValue < Threshold)
        {
            return outcomes.First(o => o.Value == OverUnderOutcomeValue.Under).Id;
        }
        else
        {
            return outcomes.First(o => o.Value == OverUnderOutcomeValue.Push).Id;
        }
    }

    public static OverUnderMarket Create(Match match, MatchEventType settlingEvent, decimal threshold, Metric metric, List<OverUnderOutcome> outcomes)
    {
        return new OverUnderMarket(new MarketId(), match.Id, settlingEvent, outcomes, threshold, metric, null);
    }
}