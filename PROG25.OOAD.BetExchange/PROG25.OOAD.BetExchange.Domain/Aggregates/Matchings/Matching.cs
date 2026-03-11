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
        Id = new MatchingId();
    }

    public MatchingId Id { get; }
    public BetId BackBetId { get; }
    public BetId LayBetId { get; }
}