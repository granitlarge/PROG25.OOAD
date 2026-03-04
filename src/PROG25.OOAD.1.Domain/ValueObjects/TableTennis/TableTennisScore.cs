using PROG25.OOAD.Domain.ValueObjects;

public record TableTennisScore : Score
{
    public int Points { get; init; }

    public TableTennisScore(int points)
    {
        if (points < 0)
            throw new ArgumentException("Points cannot be negative.", nameof(points));
        Points = points;
    }

    public override int CompareTo(Score? other)
    {
        if (other is not TableTennisScore otherScore)
            throw new ArgumentException("Cannot compare TableTennisScore with a different type of score.", nameof(other));

        return Points.CompareTo(otherScore.Points);
    }

    public override Score Add(Score other)
    {
        if (other is not TableTennisScore otherScore)
            throw new ArgumentException("Cannot add TableTennisScore with a different type of score.", nameof(other));

        return new TableTennisScore(Points + otherScore.Points);
    }
}