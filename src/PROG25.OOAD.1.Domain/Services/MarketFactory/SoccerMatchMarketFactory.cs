using PROG25.OOAD.Domain.Aggregates.Markets;
using PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.Domain.Aggregates.Matches;
using PROG25.OOAD.Domain.Services.OddsServices;
using PROG25.OOAD.Domain.ValueObjects;
using PROG25.OOAD.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.Domain.Services.MarketFactory;

public class SoccerMatchMarketFactory
{
    private readonly SoccerMatchOddsService _soccerOddsService;

    public SoccerMatchMarketFactory(SoccerMatchOddsService soccerOddsService)
    {
        _soccerOddsService = soccerOddsService;
    }

    public List<Market> UpdateMarkets(SoccerMatch match, List<Market> existingMarkets)
    {
        // Which markets can we create that don't exist yet?
        // Which existing markets need to be updated based on the new match state?
        throw new NotImplementedException();
    }

    public List<Market> CreateSoccerMarkets(SoccerMatch match)
    {
        return [.. new List<MarketType>
        {
            MarketType.NextTeamToScore,
            MarketType.OverUnder,
            MarketType.Winner
        }
        .Where(match.CanCreateMarket)
        .Select(matchType => matchType switch
        {

            MarketType.OverUnder => (Market)OverUnderMarket.Create(match, MatchEventType.MatchEnded, 2.5m, new TotalsMetric(MetricType.Goals), _soccerOddsService.CalculateTotalScoreOverUnderOdds(match, 2.5m)),

            MarketType.Winner => WinnerMarket.Create(match.Id, _soccerOddsService.CalculateMatchWinnerOdds(match)),

            _ => throw new NotSupportedException($"Market type {matchType} is not supported for creation.")
        })];
    }

    private List<Market> CreateTableTennisMarkets(TableTennisMatch match)
    {
        // Implement logic to create table tennis-specific markets based on the match details
        throw new NotImplementedException();
    }
}