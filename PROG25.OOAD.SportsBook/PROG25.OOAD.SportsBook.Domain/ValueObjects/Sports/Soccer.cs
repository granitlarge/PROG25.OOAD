using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Sports;

public static class Soccer
{
    public static readonly MetricDefinition Goals = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Goals"
    );

    public static readonly MetricDefinition Assists = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Assists"
    );

    public static readonly MetricDefinition YellowCards = new
    (
        minValue: 0,
        maxValue: 2,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Yellow Cards"
    );

    public static readonly MetricDefinition RedCards = new
    (
        minValue: 0,
        maxValue: 1,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Red Cards"
    );

    public static readonly MetricDefinition Corners = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Corners"
    );

    public static readonly MetricDefinition Penalties = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Penalties"
    );

    public static readonly MetricDefinition Period = new
    (
        minValue: 0,
        maxValue: 6,
        ScopeType.Event,
        AggregationType.None,
        FaultTolerance.Zero,
        "Period"
    );

    public static readonly MetricDefinition IsWinner = new
    (
        minValue: 0,
        maxValue: 1,
        ScopeType.Team,
        AggregationType.None,
        FaultTolerance.Zero,
        "IsWinner"
    );

    public static readonly MetricDefinition[] AllMetrics =
    [
        Goals,
        Assists,
        YellowCards,
        RedCards,
        Corners,
        Penalties,
        Period,
        IsWinner
    ];
}