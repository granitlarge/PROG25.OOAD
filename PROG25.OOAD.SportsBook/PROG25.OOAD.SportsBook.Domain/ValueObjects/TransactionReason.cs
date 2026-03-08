namespace PROG25.OOAD.SportsBook.Domain.ValueObjects;

public enum TransactionReason
{
    ExternalTransfer,
    BetSettlement,
    BetPlacement,
    BetCancellation,
    ManualAdjustment,
    Refund,
}