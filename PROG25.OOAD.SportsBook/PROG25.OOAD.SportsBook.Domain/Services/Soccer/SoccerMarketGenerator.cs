using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Services.Soccer;

internal class SoccerMarketGenerator
{
    private readonly SoccerMarketOddsCalculatorService _oddsCalculator;

    internal SoccerMarketGenerator(SoccerMarketOddsCalculatorService oddsCalculator)
    {
        _oddsCalculator = oddsCalculator;
    }

    internal List<Market> GenerateSoccerMarkets(SoccerMatchEvent @event)
    {
        return GenerateTeamScopedMarkets(@event);
    }

    private List<Market> GenerateTeamScopedMarkets(SoccerMatchEvent @event)
    {
        var fullTimeResultMarkets = GenerateTeamScopedFullTimeMarkets(@event);
        return fullTimeResultMarkets;
    }

    private List<Market> GenerateTeamScopedFullTimeMarkets(SoccerMatchEvent @event)
    {
        return GenerateTeamFullTimeResultMarkets(@event);
    }

    private List<Market> GenerateTeamFullTimeResultMarkets(SoccerMatchEvent @event)
    {
        var drawWinnerMarketConfig = new EqualScopedEventMetricMarketConfiguration
        (
            ReferenceValueBasedMetric.NonNegativePoints,
            ScopeType.Team,
            NextEventFinishedTimestamp.Instance,
            "Draw"
        );

        var (yes, no) = _oddsCalculator.CalculateYesNoOutcomes(drawWinnerMarketConfig, @event.Statistics);

        var markets = new List<Market>
        {
            GenerateWinnerMarket(@event.HomeTeam.Id),
            GenerateWinnerMarket(@event.AwayTeam.Id),
            @event.CreateMarket(yes, no, drawWinnerMarketConfig)
        };

        markets.AddRange(GenerateOverUnderTotalGoalsMarket(@event.HomeTeam.Id, 0.5m, @event));
        markets.AddRange(GenerateOverUnderTotalGoalsMarket(@event.HomeTeam.Id, 1.5m, @event));
        markets.AddRange(GenerateOverUnderTotalGoalsMarket(@event.AwayTeam.Id, 0.5m, @event));
        markets.AddRange(GenerateOverUnderTotalGoalsMarket(@event.AwayTeam.Id, 1.5m, @event));

        return markets;

        OptimalEventMetricMarket GenerateWinnerMarket(TeamId teamId)
        {
            var config = new OptimalEventMetricMarketConfiguration
            (
                ReferenceValueBasedMetric.NonNegativePoints,
                new TeamScope(teamId),
                NextEventFinishedTimestamp.Instance,
                $"{teamId} to Win",
                OptimumType.Maximum
            );

            var (yes, no) = _oddsCalculator.CalculateYesNoOutcomes(config, @event.Statistics);

            return new OptimalEventMetricMarket(@event, yes, no, config);
        }

        List<Market> GenerateOverUnderTotalGoalsMarket(TeamId teamId, decimal threshold, SoccerMatchEvent @event)
        {
            var config = new OverUnderEventMetricMarketConfiguration
            (
                ReferenceValueBasedMetric.NonNegativePoints,
                new TeamScope(teamId),
                NextEventFinishedTimestamp.Instance,
                $"Over {threshold} Points for {teamId}",
                new ReferenceValueDecimalComparer(threshold, new FaultTolerance(0))
            );

            var (over, under, push) = _oddsCalculator.CalculateOverUnderOutcomes(config, @event.Statistics);

            return
            [
                @event.CreateMarket( over, under, push , config),
            ];
        }
    }
}