using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public class OverUnderOutcome(OutcomeId id, string name, Odds odds, OverUnderOutcomeValue value) : GenericOutcome<OverUnderOutcomeValue>(id, name, odds, value)
{

}

public enum OverUnderOutcomeValue
{
    Over,
    Under,
    Push
}