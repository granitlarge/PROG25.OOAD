using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;

public class Bet
{
    protected Bet
    (
        CustomerId customerId,
        MarketId marketId,
        OutcomeId outcomeId,
        Odds odds,
        Money stake
    )
    {
        if (stake <= Money.Zero(stake.Currency))
        {
            throw new ArgumentException("Bet stake must be greater than zero.");
        }

        Id = new BetId();
        CustomerId = customerId;
        Stake = stake;
        PlacedAt = DateTimeOffset.UtcNow;
        Status = BetStatus.Placed;
    }

    public BetId Id { get; }
    public CustomerId CustomerId { get; }
    public Money Stake { get; }
    public DateTimeOffset PlacedAt { get; }
    public BetStatus Status { get; private set; }
    public MarketId MarketId { get; }
    public OutcomeId OutcomeId { get; }
    public Odds Odds { get; }
    public Money PotentialPayout => Stake * Odds.Value;

    public void SettleBetLeg(OutcomeId? winningOutcomeId)
    {
        if (Status != BetStatus.Placed)
        {
            throw new InvalidOperationException("Only placed bets can be settled.");
        }

        Status = winningOutcomeId == OutcomeId ? BetStatus.Won : BetStatus.Lost;
    }

    public void Void()
    {
        if (Status != BetStatus.Placed)
        {
            throw new InvalidOperationException("Only placed bets can be voided.");
        }
        Status = BetStatus.Voided;
    }

    internal static Bet Create(CustomerId customerId, Money stake, MarketId marketId, OutcomeId outcomeId, Odds odds)
    {
        return new Bet(customerId, marketId, outcomeId, odds, stake);
    }
}