using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps.Abstractions;

/// <summary>
/// A timestamp that is based on the current state of the event data, without comparing to previous data.
/// </summary>
public abstract record EventDataTimestamp
{
    protected EventDataTimestamp(EventDataTimestampType type)
    {
        Type = type;
    }

    public EventDataTimestampType Type { get; }
    public abstract bool HasOccurred(EventData currentEventData);
}