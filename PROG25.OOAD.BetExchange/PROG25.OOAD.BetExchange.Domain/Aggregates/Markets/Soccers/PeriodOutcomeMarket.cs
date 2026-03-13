using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Events;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Soccers;

public static class PeriodOutcomeMarket
{
    public static List<EventMetricMarket> Create(Event e)
    {
        var period = e.Type.GetPeriod(e.Data);
        if (period is null)
            return [];
        return CreateWinAndDrawMarketsForPeriod(e, period);
    }

    private static List<EventMetricMarket> CreateMarketsForPeriod(Event e, Period period)
    {
        var markets = new List<EventMetricMarket>();
        if (period.WinnerRule is not null)
        {
            markets.AddRange(CreateWinAndDrawMarketsForPeriod(e, period));
        }

        foreach (var child in period.Children)
        {
            markets.AddRange(CreateWinAndDrawMarketsForPeriod(e, child));
        }

        return markets;
    }

    private static List<EventMetricMarket> CreateOverUnderMarketsforPeriod(Event e, Period period)
    {
        // Dimensions: Period, Second, TeamId, PlayerId
        // Metrics: All of them...
        throw new NotImplementedException();
    }

    private static List<EventMetricMarket> CreateWinAndDrawMarketsForPeriod(Event e, Period period)
    {
        var markets = new List<EventMetricMarket>();
        if (period.WinnerRule is null)
        {
            return [];
        }

        foreach (var team in e.Teams)
        {
            var metric = period.WinnerRule.Metric;
            var timestamp = period.EndTimestamp;
            var name = $"'{team.Name}' to win '{period.Name}'";
            var optimumType = period.WinnerRule.OptimumType;
            var dimensionFilters = period.WinnerRule.DimensionFilters;
            var config = new BooleanEventMetricMarketConfiguration
            (
                new IsDimensionCombinationOptimalMetricExpression
                (
                    metric,
                    dimensionFilters,
                    new DimensionFilter
                    (
                        new Dictionary<string, object>
                        {
                            { Sport.TeamIdDimensionName, team.Id }
                        }.ToImmutableDictionary(),
                        metric.Dimension
                    ),
                    optimumType
                ),
                true,
                timestamp,
                name
            );

            markets.Add(e.CreateMarket(config));
        }

        var drawConfig = new BooleanEventMetricMarketConfiguration
        (
            new AllDimensionCombinationsAreEqualMetricExpression
            (
                period.WinnerRule.Metric,
                period.WinnerRule.DimensionFilters,
                [Sport.TeamIdDimensionName],
                e.Teams.Count
            ),
            true,
            period.EndTimestamp,
            $"Draw in '{period.Name}'"
        );

        markets.Add(e.CreateMarket(drawConfig));

        return markets;
    }
}