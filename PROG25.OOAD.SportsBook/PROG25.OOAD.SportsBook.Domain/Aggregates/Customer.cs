using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates;

public class Customer
{
    private Customer
    (
        PersonId personId,
        Currency currency,
        Name name,
        EmailAddress email
    )
    {
        if (personId.Type == PersonIdType.Anonymized)
        {
            throw new ArgumentException("PersonId cannot be of type Anonymized when registering a new customer.", nameof(personId));
        }

        Id = new CustomerId();
        PersonId = personId;
        Account = Account.Open(currency);
        Rename(name);
        ChangeEmail(email);
        SelfExclusion = SelfExclusionInactive.Instance;
        IsAnonymized = false;
        PlacedBetsCount = 0;
    }

    public CustomerId Id { get; }
    public PersonId PersonId { get; private set; }
    public Account Account { get; }
    public Name Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public SelfExclusion SelfExclusion { get; private set; }
    public bool IsAnonymized { get; private set; }
    public int PlacedBetsCount { get; private set; }

    public void Anonymize()
    {
        EnsureNotAnonymized();
        EnsureAccountBalanceIsZero();
        EnsureAllBetsSettledOrCancelled();

        PersonId = AnonymizedPersonId.Instance;
        Name = Name.Anonymized;
        ChangeEmail(EmailAddress.Anonymized);
        IsAnonymized = true;
    }

    public void ChangeEmail(EmailAddress newEmail)
    {
        EnsureNotAnonymized();

        if (newEmail == EmailAddress.Anonymized)
        {
            throw new ArgumentException("Email cannot be changed to an anonymized value.", nameof(newEmail));
        }

        Email = newEmail;
    }

    public void Rename(Name newName)
    {
        EnsureNotAnonymized();
        if (newName == Name.Anonymized)
        {
            throw new ArgumentException("Name cannot be changed to an anonymized value.", nameof(newName));
        }
        Name = newName;
    }

    public void SelfExclude(DateTimeOffset end)
    {
        EnsureNotAnonymized();
        var attemptedToShortenSelfExclusion = SelfExclusion.IsActive && end < SelfExclusion.End;
        if (attemptedToShortenSelfExclusion)
        {
            throw new InvalidOperationException("Cannot shorten an active self-exclusion period.");
        }
        SelfExclusion = new SelfExclusion(end);
    }

    internal void Deposit(Money amount, TransactionReason reason)
    {
        EnsureNotAnonymized();
        var isAttemptToAddMoneyWhileSelfExclusionActive = reason == TransactionReason.ExternalTransfer && SelfExclusion.IsActive;
        if (isAttemptToAddMoneyWhileSelfExclusionActive)
        {
            throw new InvalidOperationException("Customer is not allowed to add money whilst self-exclusion is active.");
        }

        if (reason == TransactionReason.BetSettlement || reason == TransactionReason.BetCancellation)
        {
            PlacedBetsCount--;
            if (PlacedBetsCount < 0)
            {
                throw new InvalidOperationException("Placed bets count cannot be negative.");
            }
        }

        Account.Deposit(amount, reason);
    }

    internal void Withdraw(Money amount, TransactionReason reason)
    {
        EnsureNotAnonymized();
        var isAttemptToPlaceBetWhileSelfExclusionActive = reason == TransactionReason.BetPlacement && SelfExclusion.IsActive;
        if (isAttemptToPlaceBetWhileSelfExclusionActive)
        {
            throw new InvalidOperationException("Customer is not allowed to withdraw money whilst self-exclusion is active.");
        }

        if (reason == TransactionReason.BetPlacement)
        {
            PlacedBetsCount++;
        }

        Account.Withdraw(amount, reason);
    }

    public static Customer Register(bool customerWithPersonIdExists, PersonId personId, Name name, EmailAddress email, Currency accountCurrency)
    {
        if (customerWithPersonIdExists)
        {
            throw new InvalidOperationException($"A customer with the person ID {personId} already exists.");
        }
        return new Customer(personId, accountCurrency, name, email);
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