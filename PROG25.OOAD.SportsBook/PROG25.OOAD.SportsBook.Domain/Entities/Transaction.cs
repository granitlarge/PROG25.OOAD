using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class Transaction
{
    private Transaction(Money amount, TransactionType type, TransactionReason reason)
    {
        Id = new TransactionId();
        Amount = amount;
        Timestamp = DateTimeOffset.UtcNow;
        Type = type;
        Reason = reason;
    }

    public TransactionId Id { get; }
    public Money Amount { get; }
    public DateTimeOffset Timestamp { get; }
    public TransactionType Type { get; set; }
    public TransactionReason Reason { get; }

    public static Transaction Create(Money amount, TransactionType type, TransactionReason reason)
    {
        return new Transaction(amount, type, reason);
    }
}