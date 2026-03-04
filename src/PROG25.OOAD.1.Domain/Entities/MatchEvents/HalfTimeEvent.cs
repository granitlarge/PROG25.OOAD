using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public record HalfTimeEvent(MatchEventId Id, MatchId MatchId, DateTimeOffset Timestamp) : MatchEvent(MatchEventType.HalfTimeEvent, Id, MatchId, Timestamp);