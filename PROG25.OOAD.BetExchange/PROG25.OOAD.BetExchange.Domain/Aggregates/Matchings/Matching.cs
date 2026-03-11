using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Matchings;

public class Matching
{
    private Matching(EventMetricMarket market, Bet backBet, Bet layBet)
    {
        if (market.Id != backBet.MarketId || market.Id != layBet.MarketId)
        {
            throw new ArgumentException("Both bets must be for the same market as the matching.");
        }

        if (backBet.OutcomeId != market.YesOutcome.Id || layBet.OutcomeId != market.NoOutcome.Id)
        {
            throw new ArgumentException("Back bet must be on the 'yes' outcome and lay bet must be on the 'no' outcome.");
        }

        if (backBet.Stake.Currency != layBet.Stake.Currency)
        {
            throw new ArgumentException("Both bets must have the same currency.");
        }

        Id = new MatchingId();
        BackBetId = backBet.Id;
        LayBetId = layBet.Id;

        Amount = new Money(Math.Min(backBet.Stake.Value, layBet.Stake.Value), backBet.Stake.Currency);
    }

    public MatchingId Id { get; }
    public BetId BackBetId { get; }
    public BetId LayBetId { get; }
    public Money Amount { get; }

    public static Matching Create(EventMetricMarket market, Bet backBet, Bet layBet)
    {
        return new Matching(market, backBet, layBet);
    }
}