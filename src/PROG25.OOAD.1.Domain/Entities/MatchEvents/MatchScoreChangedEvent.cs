using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Entities.MatchEvents;

public record MatchScoreChangedEvent
(
    MatchEventId Id,
    DateTimeOffset Timestamp,
    MatchId MatchId,
    // Optional fields to indicate which team and player scored, and the type of score (e.g., goal, own goal, penalty)
    // If ScoringTeamId is null or ScoringPlayerId is null, it can indicate that the score change was due to a reversal.
    TeamId? ScoringTeamId,
    PlayerId? ScoringPlayerId,
    ScoreType ScoreType
) : MatchEvent(MatchEventType.ScoreChangedEvent, Id, MatchId, Timestamp);
