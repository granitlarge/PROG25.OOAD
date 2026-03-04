using PROG25.OOAD.Domain.Entities;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Aggregates;

public class Customer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Customer
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    (
        CustomerId id,
        Account account,
        string firstName,
        string lastName,
        EmailAddress email,
        bool allowedToPlay
    )
    {
        Id = id;
        Account = account;
        ChangeName(firstName, lastName);
        ChangeEmail(email);
        AllowedToPlay = allowedToPlay;
    }

    public CustomerId Id { get; }
    public Account Account { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public EmailAddress Email { get; private set; }
    public bool AllowedToPlay { get; private set; }

    public void Anonymize()
    {
        if (Account.Balance > Money.Zero(Account.Currency))
        {
            throw new InvalidOperationException("Cannot anonymize a customer with a non-zero balance.");
        }

        ChangeName("Anonymized", "User");
        ChangeEmail(EmailAddress.Anonymized);
    }

    public void ChangeEmail(EmailAddress newEmail)
    {
        Email = newEmail;
    }

    public void ChangeName(string newFirstName, string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName))
            throw new ArgumentException("First name cannot be empty.", nameof(newFirstName));
        if (string.IsNullOrWhiteSpace(newLastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(newLastName));
        FirstName = newFirstName;
        LastName = newLastName;
    }

    public void DisallowPlaying()
    {
        AllowedToPlay = false;
    }

    public void AllowPlaying()
    {
        AllowedToPlay = true;
    }

    public void Deposit(Money amount)
    {
        if (!AllowedToPlay)
            throw new InvalidOperationException("Customer is not allowed to add money when not allowed to play.");

        Account.Deposit(amount, TransactionReason.ExternalTransfer);
    }

    public void Withdraw(Money amount)
    {
        Account.Withdraw(amount, TransactionReason.ExternalTransfer);
    }

    internal void PlaceBet(Money amount)
    {
        if (!AllowedToPlay)
            throw new InvalidOperationException("Customer is not allowed to place bets when not allowed to play.");
        Account.Withdraw(amount, TransactionReason.BetPlacement);
    }

    public static Customer Register(string firstName, string lastName, EmailAddress email, Currency accountCurrency)
    {
        return new Customer(new CustomerId(), Account.Open(accountCurrency), firstName, lastName, email, allowedToPlay: true);
    }
}