using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class Account
{
    private readonly List<Transaction> _transactions;

    private Account(Currency currency)
    {
        Id = new AccountId();
        Currency = currency;
        _transactions = [];
    }

    public AccountId Id { get; }

    public Currency Currency { get; }

    public Money Balance => new Money(_transactions.Where(t => t.Type == TransactionType.Deposit).Sum(e => e.Amount.Value), Currency) -
                            new Money(_transactions.Where(t => t.Type == TransactionType.Withdrawal).Sum(e => e.Amount.Value), Currency);

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
        return new Account(currency);
    }
}