using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.Outcome;

public class GenericOutcome<T_Value> : Outcome
{
    public GenericOutcome(OutcomeId id, string name, Odds odds, T_Value? value) : base(id, name, odds)
    {
        Value = value;
    }

    public T_Value? Value { get; }
}