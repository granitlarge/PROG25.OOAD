namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public record Age
{
    public Age(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Age cannot be negative.", nameof(value));
        }

        Value = value;
    }

    public int Value { get; }
}