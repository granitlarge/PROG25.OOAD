namespace PROG25.OOAD.Domain.ValueObjects;

public enum TransactionReason
{
    ExternalTransfer,
    BetSettlement,
    BetPlacement,
    BetCancellation,
    ManualAdjustment,
    Refund,
}