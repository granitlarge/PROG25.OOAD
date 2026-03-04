using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Services.OddsServices;

public class TableTennisOddsService
{
    public List<TeamOutcome> CalculateNextScorerOdds(TableTennisMatch match)
    {
        return
        [
            new(new OutcomeId(), match.HomeTeam.Name, new Odds(1.5m), match.HomeTeam.Id),
            new(new OutcomeId(), match.AwayTeam.Name, new Odds(2.5m), match.AwayTeam.Id)
        ];
    }
}