namespace PROG25.OOAD.Domain.ValueObjects.Soccer;

public record SoccerCorners
{
    public SoccerCorners(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Corners cannot be negative.");

        Value = value;
    }

    public int Value { get; }
}