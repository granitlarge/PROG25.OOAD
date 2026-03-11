namespace PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;

public class YesNoOutcome : Outcome
{
    public static readonly YesNoOutcome Yes = new(true);
    public static readonly YesNoOutcome No = new(false);
    private YesNoOutcome(bool isYes)
        : base(isYes ? "Yes" : "No")
    {
        IsYes = isYes;
    }

    public bool IsYes { get; }
}