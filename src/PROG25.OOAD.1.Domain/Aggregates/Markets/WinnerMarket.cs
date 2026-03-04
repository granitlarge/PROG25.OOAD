using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Aggregates.Markets;

public class WinnerMarket : Market
{
    private WinnerMarket(MarketId id,
        MatchId matchId,
        List<TeamOutcome> outcomes,
        OutcomeId? settledOutcomeId)
        : base(id, matchId, MarketType.Winner, [MatchEventType.MatchEnded], outcomes.Cast<Outcome>().ToList(), settledOutcomeId)
    {
        var teamOutcomesCount = outcomes.Where(o => o.Value != null).Distinct().Count();
        var nullOutcomesCount = outcomes.Count(o => o.Value == null);

        if (teamOutcomesCount < 2 || nullOutcomesCount != 1)
        {
            throw new ArgumentException("Winner market must have at least two team outcomes and one null outcome (representing the draw).", nameof(outcomes));
        }
    }

    internal static WinnerMarket Create(MatchId matchId, List<TeamOutcome> outcomes)
    {
        return new WinnerMarket(new MarketId(), matchId, outcomes, null);
    }

    protected override OutcomeId? SettleInternal(MatchEvent matchEvent, MatchState _)
    {
        if (matchEvent is not MatchEndedEvent matchEndedEvent)
        {
            throw new ArgumentException("Invalid event type for settling Winner market.", nameof(matchEvent));
        }

        var outcomes = Outcomes.Cast<TeamOutcome>().ToList();
        var winningOutcome = matchEndedEvent.IsDraw ? outcomes.First(o => o.Value == null) : outcomes.Single(o => o.Value == matchEndedEvent.WinningTeamId);
        return winningOutcome.Id;
    }
}