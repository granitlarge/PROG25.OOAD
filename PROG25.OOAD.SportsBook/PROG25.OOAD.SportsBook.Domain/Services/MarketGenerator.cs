using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.SportsBook.Domain.Services.Soccer;

namespace PROG25.OOAD.SportsBook.Domain.Services;

public class MarketGenerator
{
    public static List<EventMetricMarket> GenerateMarketsForMatch(Event match)
    {
        switch (match.Type)
        {
            case ValueObjects.EventType.Soccer:
                var oddsCalculator = new SoccerMarketOddsCalculatorService();
                var soccerMarketFactory = new SoccerMarketGenerator(oddsCalculator);
                return soccerMarketFactory.GenerateSoccerMarkets(match as SoccerMatchEvent ?? throw new ArgumentException("Match must be of type SoccerMatch", nameof(match)));
            default:
                throw new NotSupportedException($"Match type {match.Type} is not supported for market creation.");
        }
    }
}