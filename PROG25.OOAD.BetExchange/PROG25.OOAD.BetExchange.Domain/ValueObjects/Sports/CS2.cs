using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;

public static class CS2
{
    public static readonly MetricDefinition Kills = new
    (
        minValue:0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Kills"
    );

    public static readonly MetricDefinition Deaths = new
    (  
        minValue:0,
        maxValue: decimal.MaxValue,
        ScopeType.Player,
        AggregationType.Sum,
        FaultTolerance.Zero,
        "Deaths"
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

    public static readonly MetricDefinition[] AllEventMetrics =
    [
        Kills,
        Deaths,
        Assists,
    ];
}