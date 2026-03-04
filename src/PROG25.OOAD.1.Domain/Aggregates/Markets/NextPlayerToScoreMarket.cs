using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

public class NextPlayerToScoreMarket : Market
{
    private NextPlayerToScoreMarket(MarketId id,
        MatchId matchId,
        List<PlayerOutcome> outcomes,
        OutcomeId? settledOutcomeId) : base(id, matchId, MarketType.NextPlayerToScore, [MatchEventType.ScoreChangedEvent, MatchEventType.MatchEnded], outcomes.Cast<Outcome>().ToList(), settledOutcomeId)
    {
        var distinctOutcomesCount = outcomes.Select(o => o.Value).Distinct().Count();
        var nullOutcomesCount = outcomes.Count(o => o.Value == null);
        if (distinctOutcomesCount < 3 || nullOutcomesCount != 1)
        {
            throw new ArgumentException("Next Player To Score market must have three distinct outcomes: first player, second player, and no more goals.", nameof(outcomes));
        }
    }

    protected override OutcomeId? SettleInternal(MatchEvent matchEvent, MatchState _)
    {
        switch (matchEvent)
        {
            case MatchScoreChangedEvent scoreChangedEvent:
                var scoringPlayerId = scoreChangedEvent.ScoringPlayerId;
                if (scoringPlayerId == null)
                    return null;
                var outcome = Outcomes.OfType<PlayerOutcome>().Single(o => o.Value == scoringPlayerId);
                return outcome.Id;
            case MatchEndedEvent _:
                return Outcomes.OfType<PlayerOutcome>().First(o => o.Value == null).Id;
        }

        throw new ArgumentException("Invalid event type for settling Next Player To Score market.", nameof(matchEvent));
    }

    internal static NextPlayerToScoreMarket Create(Match match, List<PlayerOutcome> outcomes)
    {
        var matchPlayers = match.Teams.SelectMany(team => team.Players).Select(player => player.Id).ToHashSet();
        if (outcomes.Any(outcome => outcome.Value != null && !matchPlayers.Contains(outcome.Value)))
        {
            throw new ArgumentException("All outcomes must correspond to players in the match (except the null outcome which represents no more goals).", nameof(outcomes));
        }
        return new NextPlayerToScoreMarket(new MarketId(), match.Id, outcomes, null);
    }
}