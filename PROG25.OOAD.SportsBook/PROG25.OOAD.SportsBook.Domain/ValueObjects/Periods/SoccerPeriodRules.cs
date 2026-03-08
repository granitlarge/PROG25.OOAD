using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Sports;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Timestamps;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Periods;

public record SoccerPeriodRules : PeriodRules
{
    private static readonly WinnerRule MaxiumTeamGoalsWinnerRule = new
    (
        Soccer.Goals,
        ScopeType.Team,
        OptimumType.Maximum
    );

    private static readonly Period FirstHalf = new
    (
        "First half",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 0, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 1, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period SecondHalf = new
    (
        "Second half",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 1, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 2, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule
    );

    private static Period FullTime(ImmutableHashSet<Period> children) => new
    (
        "Full time",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 0, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 2, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule,
        children
    );

    private static Period OT(ImmutableHashSet<Period> children) => new
    (
        "OT",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 2, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 4, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule,
        children
    );

    private static readonly Period OT1 = new
    (
        "OT 1",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 2, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 3, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period OT2 = new
    (
        "OT 2",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 3, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 4, ComparisonResult.GreaterThan),
        MaxiumTeamGoalsWinnerRule
    );

    private static readonly Period Penalties = new
    (
        "Penalties",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 4, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 5, ComparisonResult.GreaterThan)
    );

    private static readonly Period SuddenDeath = new
    (
        "Sudden death",
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 5, ComparisonResult.GreaterThan),
        new EventMetricChangedComparedToReferenceValueTimestamp(EventScope.Instance, Soccer.Period, 6, ComparisonResult.GreaterThan)
    );

    public override Period? GetPeriod(EventData eventData)
    {
        var (_, status, _) = eventData;
        if (status == EventStatus.Finished || status == EventStatus.Cancelled)
        {
            return null;
        }

        var period = eventData.Metrics.Extract(EventScope.Instance, Soccer.Period).Value;
        bool? isTied = period > 0 && period <= 4 ? eventData.Metrics.ExtractAll(ScopeType.Team, Soccer.Goals).Select(mv => mv.Value).Distinct().Count() == 1 : null;

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
            new WinnerRule(Soccer.IsWinner, ScopeType.Team, OptimumType.Maximum),
            [.. children.Where(child => child != null).Cast<Period>()]
        );
    }
}