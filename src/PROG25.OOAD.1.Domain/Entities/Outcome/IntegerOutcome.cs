using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public class ExactIntegerOutcome(OutcomeId id, string name, Odds odds, int number) : GenericOutcome<int>(id, name, odds, number)
{
}
