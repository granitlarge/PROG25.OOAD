using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public class TeamOutcome(OutcomeId id, string name, Odds odds, TeamId? teamId) : GenericOutcome<TeamId>(id, name, odds, teamId)
{
}