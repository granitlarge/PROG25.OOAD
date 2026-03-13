using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;

public record SoccerPeriodRules : PeriodRules
{
    public static WinnerRule MaximumTeamGoalsWinnerRule(params int[] periodIndex) => new
    (
        Soccer.Goals,
        [Sport.TeamIdDimensionName],
        [
            ..
            periodIndex.Select
            (
                pi =>
                    new DimensionFilter
                    (
                        new Dictionary<string,object>
                        {
                            { Sport.PeriodDimensionName, pi }
                        }.ToImmutableDictionary(),
                        Soccer.Goals.Dimension
                    )
            )
        ],
        OptimumType.Maximum
    );

    public static Period PeriodFactory
    (
        string periodName,
        int periodStart,
        int periodEnd,
        ImmutableHashSet<Period>? children = null
    ) => new
    (
        periodName,
        new BooleanMetricExpressionTimestamp
        (
            new ComparisonBooleanMetricExpression
            (
                new DimensionLessMetricExpression(Soccer.Period),
                new ConstantMetricExpression<decimal>(periodStart),
                FaultTolerance.Zero,
                ComparisonResult.GreaterThan
            ),
            true
        ),
        new BooleanMetricExpressionTimestamp
        (
            new ComparisonBooleanMetricExpression
            (
                new DimensionLessMetricExpression(Soccer.Period),
                new ConstantMetricExpression<decimal>(periodEnd),
                FaultTolerance.Zero,
                ComparisonResult.GreaterThan
            ),
            true
        ),
        MaximumTeamGoalsWinnerRule([.. Enumerable.Range(periodStart + 1, periodEnd - periodStart)]),
        children
    );

    private static readonly Period FirstHalf = PeriodFactory("First Half", 0, 1);

    private static readonly Period SecondHalf = PeriodFactory("Second half", 1, 2);

    private static Period FullTime(ImmutableHashSet<Period> children) => PeriodFactory("Full time", 0, 2, children);

    private static readonly Period OT1 = PeriodFactory("OT 1", 2, 3);

    private static readonly Period OT2 = PeriodFactory("OT 2", 3, 4);

    private static Period OverTime(ImmutableHashSet<Period> children) => PeriodFactory("Over time", 2, 4, children);

    private static readonly Period Penalties = PeriodFactory("Penalties", 4, 5);

    private static readonly Period SuddenDeath = PeriodFactory("Sudden death", 5, 6);

    public override Period? GetPeriod(EventData eventData)
    {
        var (_, status, _) = eventData;
        if (status == EventStatus.Finished || status == EventStatus.Cancelled)
        {
            return null;
        }

        var period = eventData.Metrics.GetByDefinition(Soccer.Period).Single().Value;
        var goals = eventData.Metrics.GetByDefinition(Soccer.Goals);

        var goalsTotal = Soccer.Goals.Aggregate(Soccer.Goals.GroupBy([], goals).Single().Values);
        var isTied = goalsTotal == 0 || Soccer.Goals.GroupBy([Sport.TeamIdDimensionName], goals).Count == 2 &&
                                        Soccer.Goals.GroupBy([Sport.TeamIdDimensionName], goals)
                                        .Select(g => Soccer.Goals.Aggregate(g.Values))
                                        .Distinct()
                                        .Count() == 1;

        var firstHalf = period <= 1 ? FirstHalf : null;
        var secondHalf = period <= 2 ? SecondHalf : null;
        var fullTime = period <= 2 ? FullTime([.. new[] { firstHalf, secondHalf }.Where(e => e != null).Cast<Period>()]) : null;

        var ot1 = period == 2 && isTied == true || period == 3 ? OT1 : null;
        var ot2 = period == 2 && isTied == true || period == 3 || period == 4 ? OT2 : null;

        var ot = period == 2 && isTied == true || period >= 3 && period <= 4 ? OverTime([.. new[] { ot1, ot2 }.Where(e => e != null).Cast<Period>()]) : null;

        var penalties = period == 4 && isTied == true || period == 5 ? Penalties : null;

        var suddenDeath = period == 6 ? SuddenDeath : null;

        List<Period?> children =
        [
            fullTime,
            ot,
            penalties,
            suddenDeath
        ];

        return new Period
        (
            "Match",
            new NextEventStatusChangedTimestamp(EventStatus.InProgress),
            new NextEventStatusChangedTimestamp(EventStatus.Finished),
            new WinnerRule
            (
                Soccer.Goals,
                [Sport.TeamIdDimensionName],
                [],
                OptimumType.Maximum
            ),
            [.. children.Where(child => child != null).Cast<Period>()]
        );
    }
}