namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public record FaultTolerance
{
    public static readonly FaultTolerance Zero = new(0);
    public FaultTolerance(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Fault tolerance must be non-negative.", nameof(value));
        Value = value;
    }

    public decimal Value { get; }

    public ComparisonResult Compare(decimal firstValue, decimal secondValue)
    {
        var delta = Math.Abs(firstValue - secondValue);
        var isChanged = delta > Value;
        if (!isChanged)
            return ComparisonResult.Equal;
        return firstValue > secondValue ? ComparisonResult.GreaterThan : ComparisonResult.LessThan;
    }
}

public enum ComparisonResult
{
    GreaterThan,
    LessThan,
    Equal,
}