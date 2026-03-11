using PROG25.OOAD.BetExchange.Domain.ValueObjects.Oddss;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public record Money
{
    public static Money Zero(Currency currency) => new(0, currency);
    public static readonly Money ZeroEuro = Zero(Currency.Euro);

    public Money(decimal value, Currency currency)
    {
        if (value < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(value));

        Value = value;
        Currency = currency;
    }

    public decimal Value { get; init; }
    public Currency Currency { get; init; }

    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new Money(a.Value + b.Value, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new Money(a.Value - b.Value, a.Currency);
    }

    public static Money operator *(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new Money(a.Value * b.Value, a.Currency);
    }

    private static void EnsureSameCurrency(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new ArgumentException("Currency mismatch.");
    }

    public static bool operator <(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value < b.Value;
    }

    public static bool operator >(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value > b.Value;
    }

    public static bool operator <=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value <= b.Value;
    }

    public static bool operator >=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value >= b.Value;
    }

    public static Money operator *(Money a, decimal multiplier)
    {
        return new Money(a.Value * multiplier, a.Currency);
    }

    public static Money operator *(Money a, Odds b)
    {
        return new Money(a.Value * b.Value, a.Currency);
    }
}