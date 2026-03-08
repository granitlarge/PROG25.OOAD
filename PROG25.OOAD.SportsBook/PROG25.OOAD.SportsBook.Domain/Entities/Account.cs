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

    public Money Balance => new Money(_transactions.Where(t => t.Type == TransactionType.Credit).Sum(e => e.Amount.Value), Currency) -
                            new Money(_transactions.Where(t => t.Type == TransactionType.Debit).Sum(e => e.Amount.Value), Currency);

    public IReadOnlyList<Transaction> Transactions => _transactions;

    internal void Credit(Money amount, TransactionReason reason)
    {
        EnsureSameCurrency(amount);
        _transactions.Add(Transaction.Create(amount, TransactionType.Credit, reason));
    }

    internal void Debit(Money amount, TransactionReason reason)
    {
        EnsureSameCurrency(amount);

        if (amount.Value > Balance.Value)
            throw new InvalidOperationException("Insufficient funds.");

        _transactions.Add(Transaction.Create(amount, TransactionType.Debit, reason));
    }

    internal Money GetDepositedAmountInPeriod(DateTimeOffset start, DateTimeOffset end)
    {
        if (end < start)
        {
            throw new ArgumentException("End must be greater or equal to start", nameof(end));
        }

        var amount = Transactions
        .Where(t => t.Type == TransactionType.Credit)
        .Where(t => t.Reason == TransactionReason.ExternalTransfer)
        .Where(t => t.Timestamp >= start && t.Timestamp <= end)
        .Sum(t => t.Amount.Value);

        return new Money(amount, Currency);
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