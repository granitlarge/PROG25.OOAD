using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Aggregates.Markets;

public class NextTeamToScoreMarket : Market
{
    private NextTeamToScoreMarket(MarketId id,
        MatchId matchId,
        List<TeamOutcome> outcomes,
        OutcomeId? settledOutcomeId) : base(id, matchId, MarketType.NextTeamToScore, [MatchEventType.ScoreChangedEvent, MatchEventType.MatchEnded], outcomes.Cast<Outcome>().ToList(), settledOutcomeId)
    {
        var distinctOutcomesCount = outcomes.Select(o => o.Value).Distinct().Count();
        var nullOutcomesCount = outcomes.Count(o => o.Value == null);
        if (distinctOutcomesCount < 3 || nullOutcomesCount != 1)
        {
            throw new ArgumentException("Next Team To Score market must have three distinct outcomes: first team, second team, and no more goals.", nameof(outcomes));
        }
    }

    protected override OutcomeId? SettleInternal(MatchEvent matchEvent, MatchState _)
    {
        switch (matchEvent)
        {
            case MatchScoreChangedEvent scoreChangedEvent:
                var scoringTeamId = scoreChangedEvent.ScoringTeamId;
                var outcome = Outcomes.OfType<TeamOutcome>().Single(o => o.Value == scoringTeamId);
                break;
            case MatchEndedEvent _:
                return Outcomes.OfType<TeamOutcome>().First(o => o.Value == null).Id;
        }
        throw new ArgumentException("Invalid event type for settling Next Team To Score market.", nameof(matchEvent));
    }

    internal static NextTeamToScoreMarket Create(Match match, List<TeamOutcome> outcomes)
    {
        if (outcomes.Any(outcome => match.Teams.All(team => team.Id != outcome.Value)))
        {
            throw new ArgumentException("All outcomes must correspond to teams in the match.", nameof(outcomes));
        }
        return new NextTeamToScoreMarket(new MarketId(), match.Id, outcomes, null);
    }
}