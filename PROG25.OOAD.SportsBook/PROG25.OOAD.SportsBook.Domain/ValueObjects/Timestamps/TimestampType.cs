namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public enum TimestampType
{
    ElapsedMatchTimeExceeded,
    /// <summary>
    /// Represents a timestamp that occurs when a specified relative time has been exceeded since a given start time.
    /// This can be used to determine bets that occur some time after the bet was placed, such as "what will happen 10 minutes after the bet is placed?"
    /// </summary>
    RelativeTimeExceeded,
    NextMetricChange, // e.g., next goal, next point, etc.
    MatchFinished,
    MatchStatusChanged // e.g., match starts, halftime, etc.
}