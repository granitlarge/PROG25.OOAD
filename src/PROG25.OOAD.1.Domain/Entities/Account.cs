using PROG25.OOAD.Domain.Extensions;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities;

public class Account
{
    private readonly List<Transaction> _transactions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Account() {} // EF
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Account(AccountId id, Currency currency, List<Transaction> transactions)
    {
        Id = id;
        Currency = currency;
        _transactions = [.. transactions];
    }

    public AccountId Id { get; }

    public Currency Currency { get; }

    public Money Balance => _transactions.Where(t => t.Type == TransactionType.Deposit).Sum(Currency) -
                              _transactions.Where(t => t.Type == TransactionType.Withdrawal).Sum(Currency);

    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    internal void Deposit(Money amount, TransactionReason reason)
    {
        EnsureSameCurrency(amount);
        _transactions.Add(Transaction.Create(amount, TransactionType.Deposit, reason));
    }

    internal void Withdraw(Money amount, TransactionReason reason)
    {
        EnsureSameCurrency(amount);

        if (amount.Value > Balance.Value)
            throw new InvalidOperationException("Insufficient funds.");

        _transactions.Add(Transaction.Create(amount, TransactionType.Withdrawal, reason));
    }

    private void EnsureSameCurrency(Money amount)
    {
        if (amount.Currency != Currency)
            throw new ArgumentException("Currency mismatch.", nameof(amount));
    }

    internal static Account Open(Currency currency)
    {
        return new Account(new AccountId(), currency, []);
    }
}