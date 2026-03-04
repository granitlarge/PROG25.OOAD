using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public abstract record Timestamp
{
    protected Timestamp(TimestampType type)
    {
        Type = type;
    }

    public TimestampType Type { get; }

    public abstract bool HasOccured(EventStatistics previousMatchState, EventStatistics currentMatchState);
    public abstract bool HasOccured(EventStatistics currentMatchState);
}