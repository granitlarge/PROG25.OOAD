using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;

public static class Soccer
{
    private static readonly DimensionDefinition EmptyDimension = new(ImmutableDictionary<string, Type>.Empty);
    private static readonly DimensionDefinition PeriodSecondTeamPlayerDimension = new
    (
        new Dictionary<string, Type>
        {
            [Sport.PeriodDimensionName] = typeof(string),
            [Sport.SecondDimensionName] = typeof(int),
            [Sport.TeamIdDimensionName] = typeof(TeamId),
            [Sport.PlayerIdDimensionName] = typeof(PlayerId)
        }.ToImmutableDictionary()
    );

    public static readonly MetricDefinition Goals = new
    (
        0,
        decimal.MaxValue,
        FaultTolerance.Zero,
        "Goals",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition Assists = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        FaultTolerance.Zero,
        "Assists",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition YellowCards = new
    (
        minValue: 0,
        maxValue: 2,
        FaultTolerance.Zero,
        "Yellow Cards",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition RedCards = new
    (
        minValue: 0,
        maxValue: 1,
        FaultTolerance.Zero,
        "Red Cards",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition Corners = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        FaultTolerance.Zero,
        "Corners",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition Penalties = new
    (
        minValue: 0,
        maxValue: decimal.MaxValue,
        FaultTolerance.Zero,
        "Penalties",
        PeriodSecondTeamPlayerDimension,
        Aggregation.Sum
    );

    public static readonly MetricDefinition Period = new
    (
        minValue: 0,
        maxValue: 6,
        FaultTolerance.Zero,
        "Period",
        EmptyDimension,
        Aggregation.None
    );

    public static readonly MetricDefinition[] AllMetrics =
    [
        Goals,
        Assists,
        YellowCards,
        RedCards,
        Corners,
        Penalties,
        Period
    ];
}