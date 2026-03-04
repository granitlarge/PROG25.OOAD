namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record Odds
{
    public Odds(decimal value)
    {
        if (value <= 1)
            throw new ArgumentException("Odds must be greater than one.", nameof(value));

        Value = value;
    }

    public decimal Value { get; }

    public static Odds operator *(Odds a, Odds b)
    {
        return new Odds(a.Value * b.Value);
    }
}