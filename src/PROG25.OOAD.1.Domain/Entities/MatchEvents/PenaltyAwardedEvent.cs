using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public record PenaltyAwardedEvent
(
    MatchEventId Id,
    MatchId MatchId,
    DateTimeOffset Timestamp,
    TeamId AwardedToTeamId,
    PlayerId FoulingParticipantId,
    PlayerId FouledParticipantId
) : MatchEvent(MatchEventType.PenaltyAwardedEvent, Id, MatchId, Timestamp);