namespace PROG25.OOAD.BetExchange.Domain.ValueObjects;

public enum TransactionReason
{
    ExternalTransfer,
    BetSettlement,
    BetPlacement,
    BetCancellation,
    ManualAdjustment,
    Refund,
}