using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public record MatchStartedEvent
(
    MatchEventId Id,
    DateTimeOffset Timestamp,
    MatchId MatchId
) : MatchEvent(MatchEventType.MatchStarted, Id, MatchId, Timestamp);