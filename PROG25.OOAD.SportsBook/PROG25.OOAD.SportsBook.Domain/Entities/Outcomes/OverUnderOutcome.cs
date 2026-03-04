using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;

public class OverUnderOutcome : Outcome
{
    public OverUnderOutcome
    (
        OverUnderOutcomeType type,
        Odds odds
    ) : base(Enum.GetName(type)!, odds)
    {
        Type = type;
    }

    public OverUnderOutcomeType Type { get; }
}

public enum OverUnderOutcomeType
{
    Over,
    Under,
    Push
}