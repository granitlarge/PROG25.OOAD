using PROG25.OOAD.SportsBook.Domain.ValueObjects;

namespace PROG25.OOAD.SportsBook.Domain.Entities;

public class BetLeg
{
    internal BetLeg(MarketId marketId, OutcomeId outcomeId, Odds odds)
    {
        Id = new BetLegId();
        MarketId = marketId;
        OutcomeId = outcomeId;
        Odds = odds;
        Status = BetLegStatus.Pending;
    }

    public BetLegId Id { get; }
    public MarketId MarketId { get; }
    public OutcomeId OutcomeId { get; }
    public Odds Odds { get; }
    public BetLegStatus Status { get; private set; }

    public void Settle(OutcomeId? winningOutcomeId)
    {
        if (Status != BetLegStatus.Pending)
        {
            throw new InvalidOperationException("Only pending bet legs can be settled.");
        }
        Status = OutcomeId == winningOutcomeId ? BetLegStatus.Won : BetLegStatus.Lost;
    }

    public void Void()
    {
        if (Status != BetLegStatus.Pending)
        {
            throw new InvalidOperationException("Only pending bet legs can be voided.");
        }
        Status = BetLegStatus.Void;
    }
}

public enum BetLegStatus
{
    Pending,
    Won,
    Lost,
    Void
}