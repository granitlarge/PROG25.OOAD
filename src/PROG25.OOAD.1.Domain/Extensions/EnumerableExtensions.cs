using PROG25.OOAD.Domain.Entities;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Extensions;

public static class EnumerableExtensions
{
    public static Money Sum(this IEnumerable<Transaction> transactions, Currency currency)
    {
        if (!transactions.Any())
        {
            return new Money(0, currency);
        }

        var first = transactions.First();
        var total = new Money(0, first.Amount.Currency);
        foreach (var transaction in transactions)
        {
            var amount = transaction.Amount;
            if (amount.Currency != total.Currency)
            {
                throw new ArgumentException("Currency mismatch.");
            }
            total += amount;
        }
        return total;
    }
}