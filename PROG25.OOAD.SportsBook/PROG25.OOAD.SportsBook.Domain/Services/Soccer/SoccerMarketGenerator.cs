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

    internal List<EventMetricMarket> GenerateSoccerMarkets(SoccerMatchEvent @event)
    {
        return GenerateTeamScopedMarkets(@event);
    }

    private List<EventMetricMarket> GenerateTeamScopedMarkets(SoccerMatchEvent @event)
    {
        var fullTimeResultMarkets = GenerateTeamScopedFullTimeMarkets(@event);
        return fullTimeResultMarkets;
    }

    private List<EventMetricMarket> GenerateTeamScopedFullTimeMarkets(SoccerMatchEvent @event)
    {
        return GenerateTeamFullTimeResultMarkets(@event);
    }

    private List<EventMetricMarket> GenerateTeamFullTimeResultMarkets(SoccerMatchEvent @event)
    {
        var drawWinnerMarketConfig = new EqualScopeEventMetricMetricMarketConfiguration
        (
            ScopeType.Team,
            SoccerMetrics.Goals,
            NextEventStatusChangedTimestamp.ForStatus(EventStatus.Finished),
            "Draw"
        );

        var (yes, no) = _oddsCalculator.CalculateYesNoOutcomes(drawWinnerMarketConfig, @event.Data);

        var markets = new List<EventMetricMarket>
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

        OptimalScopedEventMetricMarket GenerateWinnerMarket(TeamId teamId)
        {
            var (scope, metric) = SoccerMetrics.GetTeamScopedMetrics(teamId).First(m => m.MetricDefinition == SoccerMetrics.Goals);
            var config = new OptimalScopedEventMetricMarketConfiguration
            (
                scope,
                metric,
                NextEventStatusChangedTimestamp.ForStatus(EventStatus.Finished),
                $"Winner {teamId}",
                OptimumType.Maximum
            );

            var (yes, no) = _oddsCalculator.CalculateYesNoOutcomes(config, @event.Data);
            return @event.CreateMarket(yes, no, config);
        }

        List<EventMetricMarket> GenerateOverUnderTotalGoalsMarket(TeamId teamId, decimal threshold, SoccerMatchEvent @event)
        {
            var (scope, metric) = SoccerMetrics.GetTeamScopedMetrics(teamId).First(m => m.MetricDefinition == SoccerMetrics.Goals);
            var config = new ComparisonScopedEventMetricMarketConfiguration
            (
                threshold,
                scope,
                metric,
                NextEventStatusChangedTimestamp.ForStatus(EventStatus.Finished),
                ComparisonResult.GreaterThan,
                $"Total Goals Over {threshold} for {teamId}"
            );

            var (yes, no) = _oddsCalculator.CalculateYesNoOutcomes(config, @event.Data);

            return
            [
                @event.CreateMarket( yes, no, config),
            ];
        }
    }
}