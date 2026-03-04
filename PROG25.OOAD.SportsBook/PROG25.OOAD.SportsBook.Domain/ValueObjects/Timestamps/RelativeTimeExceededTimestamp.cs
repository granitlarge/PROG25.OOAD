using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

/// <summary>
/// Represents a timestamp that occurs when a specified relative time has been exceeded since a given start time.
/// This can be used to determine bets that occur some time after the bet was placed, such as "what will happen 10 minutes after the bet is placed?"
/// </summary>
public record RelativeTimeExceededTimestamp : Timestamp
{
    public RelativeTimeExceededTimestamp(DateTimeOffset startTime, TimeSpan timeSpan)
        : base(TimestampType.RelativeTimeExceeded)
    {
        if (timeSpan <= TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(timeSpan), "Time span must be positive.");

        StartTime = startTime;
        TimeSpan = timeSpan;
    }

    public DateTimeOffset StartTime { get; }
    public TimeSpan TimeSpan { get; }


    public override bool HasOccured(EventStatistics _, EventStatistics __)
    {
        return HasOccurred();
    }

    public override bool HasOccured(EventStatistics _)
    {
        return HasOccurred();
    }

    private bool HasOccurred()
    {
        return (DateTimeOffset.UtcNow - StartTime) >= TimeSpan;
    }
}