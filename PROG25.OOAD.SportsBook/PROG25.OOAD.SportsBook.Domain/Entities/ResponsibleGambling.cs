using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class ResponsibleGambling
{
    public ResponsibleGambling
    (
        DepositLimits depositLimits,
        SelfExclusion selfExclusion
    )
    {
        DepositLimits = depositLimits;
        SelfExclusion = selfExclusion;
    }

    public DepositLimits DepositLimits { get; private set; }
    public SelfExclusion SelfExclusion { get; private set; }

    public void ChangeDepositLimits(DepositLimits depositLimits)
    {
#warning we should verify that the new deposit limits are larger or equal to the older deposit limits
        DepositLimits = depositLimits;
    }

    public void SelfExclude(SelfExclusion newSelfExclusion)
    {
        if (SelfExclusion.IsActive && SelfExclusion.End > newSelfExclusion.End)
        {
            throw new InvalidOperationException("Cannot shorten existing self-exclusion.");
        }

        SelfExclusion = newSelfExclusion;
    }

    public void EnsureIsAllowedDeposit
    (
        Money amountToDeposit,
        Account account,
        DateTimeOffset now
    )
    {
        if (SelfExclusion.IsActive)
        {
            throw new InvalidOperationException("Money can't be deposited while self-exclusion is active.");
        }

        foreach (var depositLimit in DepositLimits.GetAll())
        {
            var timeSpan = depositLimit.Duration;
            var allowedDepositedAmount = depositLimit.Amount;
            var start = now - timeSpan;
            var end = now;

            var depositedAmount = account.GetDepositedAmountInPeriod(start, end);

            if (depositedAmount.Value + amountToDeposit.Value > allowedDepositedAmount)
            {
                throw new InvalidOperationException("Deposit would exceed self-imposed limits");
            }
        }

    }

    public void EnsureBetPlacementIsAllowed()
    {
        if (SelfExclusion.IsActive)
        {
            throw new InvalidOperationException("Bet placement is not allowed while self-exclusion is active.");
        }
    }


}