namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events.Soccer;

public record SoccerMatchEventData
(
    SoccerMatchEventMetrics SoccerEventMetrics,
    EventStatus EventStatus,
    DateTimeOffset StartDate
) : EventData(SoccerEventMetrics, EventStatus, StartDate);