using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public record MatchCancelledEvent(
    MatchEventId Id,
    MatchId MatchId,
    DateTimeOffset Timestamp
) : MatchEvent(MatchEventType.MatchCancelled, Id, MatchId, Timestamp);