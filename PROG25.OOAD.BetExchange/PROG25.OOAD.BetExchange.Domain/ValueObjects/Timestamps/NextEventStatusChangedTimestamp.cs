using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

public record NextEventStatusChangedTimestamp : EventDataTimestamp
{
    public static NextEventStatusChangedTimestamp ForStatus(EventStatus newStatus) => new(newStatus);
    public EventStatus NewStatus { get; }

    public NextEventStatusChangedTimestamp(EventStatus newStatus)
        : base(EventDataTimestampType.NextEventDataChanged)
    {
        NewStatus = newStatus;
    }

    public override bool HasOccurred(EventData currentEventData)
    {
        return currentEventData.Status == NewStatus;
    }
}