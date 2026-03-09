using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates;

public class Customer
{
    private Customer
    (
        CustomerIdentity customerIdentity,
        ResponsibleGambling responsibleGambling,
        Currency currency
    )
    {
        Id = new CustomerId();
        Identity = customerIdentity;
        ResponsibleGambling = responsibleGambling;
        Account = Account.Open(currency);
        PlacedBetsCount = 0;
    }

    public CustomerId Id { get; }
    public CustomerIdentity Identity { get; private set;}
    public ResponsibleGambling ResponsibleGambling { get; }
    public Account Account { get; }
    public bool IsAnonymized => Identity.IsAnonymized;
    public int PlacedBetsCount { get; private set; }

    public void Anonymize()
    {
        EnsureNotAnonymized();
        EnsureAccountBalanceIsZero();
        EnsureAllBetsSettledOrCancelled();
        Identity = CustomerIdentity.Anonymized;
    }

    public void Rename(Name name)
    {
        EnsureNotAnonymized();
        Identity = CustomerIdentity.Create(Identity.PersonId, name, Identity.Email);
    }

    public void ChangeEmail(EmailAddress email)
    {
        EnsureNotAnonymized();
        Identity = CustomerIdentity.Create(Identity.PersonId, Identity.Name, email);
    }

    public void Deposit(Money amount, DateTimeOffset now)
    {
        EnsureNotAnonymized();
        ResponsibleGambling.EnsureIsAllowedDeposit(amount, Account, now);
        Account.Credit(amount, TransactionReason.ExternalTransfer);
    }

    public void PlaceBet(Money amount)
    {
        EnsureNotAnonymized();
        ResponsibleGambling.EnsureBetPlacementIsAllowed();
        PlacedBetsCount++;
        Account.Debit(amount, TransactionReason.BetPlacement);
    }

    public void SettleBet(Money payout)
    {
        EnsureNotAnonymized();
        PlacedBetsCount--;
        Account.Credit(payout, TransactionReason.BetSettlement);
    }

    public void VoidBet(Money stake)
    {
        EnsureNotAnonymized();
        PlacedBetsCount--;
        Account.Credit(stake, TransactionReason.BetCancellation);
    }

    public void Withdraw(Money amount)
    {
        EnsureNotAnonymized();
        Account.Debit(amount, TransactionReason.ExternalTransfer);
    }

    internal static Customer Create
    (
        PersonId personId,
        Name name,
        EmailAddress email,
        Currency currency,
        DepositLimits depositLimits
    )
    {
        return new Customer
        (
            CustomerIdentity.Create(personId, name, email),
            new ResponsibleGambling(depositLimits, SelfExclusionInactive.Instance),
            currency
        );
    }

    private void EnsureNotAnonymized()
    {
        if (IsAnonymized)
        {
            throw new InvalidOperationException("Operation not allowed for anonymized customers.");
        }
    }

    private void EnsureAccountBalanceIsZero()
    {
        if (Account.Balance > Money.Zero(Account.Currency))
        {
            throw new InvalidOperationException("Operation not allowed for customers with non-zero balance.");
        }
    }

    private void EnsureAllBetsSettledOrCancelled()
    {
        if (PlacedBetsCount > 0)
        {
            throw new InvalidOperationException("Operation not allowed for customers with open bets.");
        }
    }
}