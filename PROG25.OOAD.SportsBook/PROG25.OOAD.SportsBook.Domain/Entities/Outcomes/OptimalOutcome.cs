using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;

public class YesNoOutcome : Outcome
{
    public YesNoOutcome(Odds odds, bool isYes)
        : base(isYes ? "Yes" : "No", odds)
    {
        IsYes = isYes;
    }

    public bool IsYes { get; }
}