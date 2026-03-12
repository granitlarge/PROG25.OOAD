using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;

public record SoccerPeriodRules : PeriodRules
{
    private static readonly WinnerRule MaxiumTeamGoalsWinnerRule = new
    (
        Soccer.Goals,
        [Sport.TeamIdDimensionName],
        OptimumType.Maximum
    );

    private static readonly Period FirstHalf = new
    (
        "First half",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            0,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            1,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period SecondHalf = new
    (
        "Second half",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            1,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            2,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule
    );

    private static Period FullTime(ImmutableHashSet<Period> children) => new
    (
        "Full time",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            0,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            2,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule,
        children
    );

    private static Period OT(ImmutableHashSet<Period> children) => new
    (
        "OT",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            2,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            4,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule,
        children
    );

    private static readonly Period OT1 = new
    (
        "OT 1",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            2,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            3,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period OT2 = new
    (
        "OT 2",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            3,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            4,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period Penalties = new
    (
        "Penalties",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            4,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            5,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        )
    );

    private static readonly Period SuddenDeath = new
    (
        "Sudden death",
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            5,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        ),
        new MetricExpressionChangedComparedToReferenceValueTimestamp
        (
            new FilteredAndAggregatedMetricExpression(Soccer.Period, []),
            6,
            ComparisonResult.GreaterThan,
            FaultTolerance.Zero
        )
    );

    public override Period? GetPeriod(EventData eventData)
    {
        var (_, status, _) = eventData;
        if (status == EventStatus.Finished || status == EventStatus.Cancelled)
        {
            return null;
        }

        var period = eventData.Metrics.GetByDefinition(Soccer.Period).Single().Value;
        var goals = eventData.Metrics.GetByDefinition(Soccer.Goals);
        bool? isTied = period > 0 && period <= 4 ? Soccer.Goals
                                                         .GroupBy([Sport.TeamIdDimensionName], goals)
                                                         .Select(g => Soccer.Goals.Aggregate(g.Values))
                                                         .Distinct()
                                                         .Count() == 1 : null;

        var firstHalf = period <= 1 ? FirstHalf : null;
        var secondHalf = period <= 2 ? SecondHalf : null;
        var fullTime = period <= 2 ? FullTime([.. new[] { firstHalf, secondHalf }.Where(e => e != null).Cast<Period>()]) : null;

        var ot1 = period == 2 && isTied == true || period == 3 ? OT1 : null;
        var ot2 = period == 2 && isTied == true || period == 4 ? OT2 : null;

        var ot = period == 2 && isTied == true || period >= 3 && period <= 4 ? OT([.. new[] { ot1, ot2 }.Where(e => e != null).Cast<Period>()]) : null;

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
            new WinnerRule(Soccer.Goals, [Sport.TeamIdDimensionName], OptimumType.Maximum),
            [.. children.Where(child => child != null).Cast<Period>()]
        );
    }
}