using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.ValueObjects;

public record MatchEndedEvent
(
    MatchEventId Id,
    MatchId MatchId,
    DateTimeOffset Timestamp,
    TeamId? WinningTeamId
) : MatchEvent(MatchEventType.MatchEnded, Id, MatchId, Timestamp)
{
    public bool IsDraw => WinningTeamId == null;
}