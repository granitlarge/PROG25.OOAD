using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Aggregates;

public class Bet
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Bet() { } // EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Bet(BetId id, MarketId marketId, CustomerId customerId, OutcomeId outcomeId, Money amount, Odds odds, BetStatus status, bool? isWinningBet)
    {
        Id = id;
        MarketId = marketId;
        CustomerId = customerId;
        OutcomeId = outcomeId;
        Amount = amount;
        Odds = odds;
        Status = status;
        IsWinningBet = isWinningBet;
    }

    public BetId Id { get; private set; }
    public MarketId MarketId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public OutcomeId OutcomeId { get; private set; }
    public Money Amount { get; private set; }
    public Odds Odds { get; private set; }
    public BetStatus Status { get; private set; }
    public Money PotentialPayout => Amount * Odds.Value;
    public bool? IsWinningBet { get; private set; }

    internal void Settle(bool isWinningBet)
    {
        if (Status != BetStatus.Placed)
            throw new InvalidOperationException("Only placed bets can be settled.");

        Status = BetStatus.Settled;
        IsWinningBet = isWinningBet;
    }

    internal void Cancel()
    {
        if (Status != BetStatus.Placed)
        {
            throw new InvalidOperationException("Only placed bets can be cancelled.");
        }

        Status = BetStatus.Cancelled;
    }

    internal static Bet Place(MarketId marketId, CustomerId customerId, OutcomeId outcomeId, Money amount, Odds odds)
    {
        return new Bet
        (
            id: new BetId(),
            marketId: marketId,
            customerId: customerId,
            outcomeId: outcomeId,
            amount: amount,
            odds: odds,
            status: BetStatus.Placed,
            isWinningBet: null
        );
    }
}

public enum BetStatus
{
    Placed,
    Settled,
    Cancelled
}