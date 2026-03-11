using PROG25.OOAD.BetExchange.Domain.ValueObjects.Oddss;

namespace PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;

public class YesNoOutcome : Outcome
{
    public YesNoOutcome(Odds odds, bool isYes)
        : base(isYes ? "Yes" : "No", odds)
    {
        IsYes = isYes;
    }

    public bool IsYes { get; }
}