using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

public record NextEventStatusChangeTimestamp : Timestamp
{
    public EventStatus NewStatus { get; }

    public NextEventStatusChangeTimestamp(EventStatus newStatus)
        : base(TimestampType.MatchStatusChanged)
    {
        NewStatus = newStatus;
    }

    public override bool HasOccured(EventStatistics previousMatchState, EventStatistics currentMatchState)
    {
        return currentMatchState.Status == NewStatus && previousMatchState.Status != NewStatus;
    }

    public override bool HasOccured(EventStatistics currentMatchState)
    {
        return currentMatchState.Status == NewStatus;
    }
}

public record NextEventFinishedTimestamp : NextEventStatusChangeTimestamp
{
    public static readonly NextEventFinishedTimestamp Instance = new();
    private NextEventFinishedTimestamp() : base(EventStatus.Finished) { }
}