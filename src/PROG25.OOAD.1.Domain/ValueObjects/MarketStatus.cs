namespace PROG25.OOAD.Domain.ValueObjects;

public enum MarketStatus
{
    Open, // Bets can be placed
    Closed, // No more bets can be placed, but the outcome has not been determined yet
    Settled, // The outcome has been determined and bets have been settled
    Cancelled // The market has been cancelled, and all bets will be refunded
}