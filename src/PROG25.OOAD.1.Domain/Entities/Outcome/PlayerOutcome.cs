using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public class PlayerOutcome(OutcomeId id, string name, Odds odds, PlayerId? playerId) : GenericOutcome<PlayerId>(id, name, odds, playerId)
{
}