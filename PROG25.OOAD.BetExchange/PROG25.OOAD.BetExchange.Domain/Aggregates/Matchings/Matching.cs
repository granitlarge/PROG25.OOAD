using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Matchings;

public class Matching
{
    private Matching(Bet b1, Bet b2)
    {
        if (b1.OutcomeId != b2.OutcomeId || (b1.Side == b2.Side))
        {
            throw new ArgumentException("Bets must be on the same outcome but opposite sides to be matched.");
        }

        if (b1.Stake.Currency != b2.Stake.Currency)
        {
            throw new ArgumentException("Both bets must have the same currency.");
        }

        Id = new MatchingId();
        BackBetId = b1.Id;
        LayBetId = b2.Id;

        Amount = new Money(Math.Min(b1.Stake.Value, b2.Stake.Value), b1.Stake.Currency);
    }

    public MatchingId Id { get; }
    public BetId BackBetId { get; }
    public BetId LayBetId { get; }
    public Money Amount { get; }

    public static Matching Create(Bet backBet, Bet layBet)
    {
        return new Matching(backBet, layBet);
    }
}