namespace PROG25.OOAD.Domain.ValueObjects;

public record SoccerScore
{
    public SoccerScore(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Goals cannot be negative.");

        Value = value;
    }

    public static bool operator <(SoccerScore score1, SoccerScore score2) =>
        score1.Value < score2.Value;

    public static bool operator >(SoccerScore score1, SoccerScore score2) =>
        score1.Value > score2.Value;

    public static SoccerScore operator +(SoccerScore score1, SoccerScore score2) =>
        new(score1.Value + score2.Value);

    public int Value { get; }
}