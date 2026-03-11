namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

public record EventData
(
    EventMetrics Metrics,
    EventStatus Status,
    DateTimeOffset StartDate
);