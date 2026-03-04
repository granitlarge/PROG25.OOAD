using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Services.OddsServices;

public class SoccerMatchOddsService
{
    public List<PlayerOutcome> CalculateNextScorerOdds(SoccerMatch match)
    {
        return match.HomeTeam.Players.Concat(match.AwayTeam.Players)
            .Select(player => new PlayerOutcome(new OutcomeId(), player.Name, new Odds(3.0m), player.Id))
            .ToList();
    }

    public List<TeamOutcome> CalculateMatchWinnerOdds(SoccerMatch match)
    {
        return [
            new(new OutcomeId(), match.HomeTeam.Name, new Odds(1.6m), match.HomeTeam.Id),
            new(new OutcomeId(), match.AwayTeam.Name, new Odds(2.4m), match.AwayTeam.Id)
        ];
    }

    public List<OverUnderOutcome> CalculateTeamScoreOverUnderOdds(SoccerMatch match, decimal score)
    {
        return
        [
            new(new OutcomeId(), "Over", new Odds(1.9m), OverUnderOutcomeValue.Over),
            new(new OutcomeId(), "Under", new Odds(1.9m), OverUnderOutcomeValue.Under)
        ];
    }

    public List<OverUnderOutcome> CalculateTotalScoreOverUnderOdds(SoccerMatch match, decimal score)
    {
        return
        [
            new(new OutcomeId(), "Over", new Odds(1.9m), OverUnderOutcomeValue.Over),
            new(new OutcomeId(), "Under", new Odds(1.9m), OverUnderOutcomeValue.Under)
        ];
    }
}