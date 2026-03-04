using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities;

public class Transaction
{
    private Transaction(TransactionId id, Money amount, DateTimeOffset timestamp, TransactionType type, TransactionReason reason)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
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
        return new Transaction(new TransactionId(), amount, DateTimeOffset.UtcNow, type, reason);
    }
}