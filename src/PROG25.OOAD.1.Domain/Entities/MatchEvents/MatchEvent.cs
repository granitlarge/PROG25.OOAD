using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public abstract record MatchEvent
(
    MatchEventType Type,
    MatchEventId Id,
    MatchId MatchId,
    DateTimeOffset Timestamp
);