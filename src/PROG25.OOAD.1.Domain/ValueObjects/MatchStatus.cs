namespace PROG25.OOAD.Domain.ValueObjects;

public enum MatchStatus
{
    Scheduled, // Match is scheduled but has not started yet
    InProgress, // Match has started and is currently in progress
    Ended, // Match has ended and the outcome is known
    Cancelled // Match has been cancelled, and all bets will be refunded
}