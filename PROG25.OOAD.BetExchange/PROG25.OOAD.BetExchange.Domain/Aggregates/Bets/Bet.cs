using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;

public class Bet
{
    protected Bet
    (
        CustomerId customerId,
        MarketId marketId,
        OutcomeId outcomeId,
        Money stake
    )
    {
        if (stake <= Money.Zero(stake.Currency))
        {
            throw new ArgumentException("Bet stake must be greater than zero.");
        }

        if (stake.Currency != Currency.Euro)
        {
            throw new ArgumentException("Bet stake must be in Euros.");
        }

        Id = new BetId();
        CustomerId = customerId;
        Stake = stake;
        MarketId = marketId;
        OutcomeId = outcomeId;
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

    public void Settle(OutcomeId? winningOutcomeId)
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

    internal static Bet Create(CustomerId customerId, Money stake, MarketId marketId, OutcomeId outcomeId)
    {
        return new Bet(customerId, marketId, outcomeId, stake);
    }
}