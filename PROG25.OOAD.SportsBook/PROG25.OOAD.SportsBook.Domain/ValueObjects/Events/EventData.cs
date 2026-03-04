namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

public record EventData
(
    EventMetrics Metrics,
    EventStatus Status,
    DateTimeOffset StartDate
);