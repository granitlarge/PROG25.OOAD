using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class CS2Metrics
{
    public static readonly MetricDefinition Kills = new(0, decimal.MaxValue, FaultTolerance.Zero, "Kills", [ScopeType.Player, ScopeType.Team, ScopeType.Event]);
    public static readonly MetricDefinition Deaths = new(0, decimal.MaxValue, FaultTolerance.Zero, "Deaths", [ScopeType.Player, ScopeType.Team, ScopeType.Event]);
    public static readonly MetricDefinition Assists = new(0, decimal.MaxValue, FaultTolerance.Zero, "Assists", [ScopeType.Player, ScopeType.Team, ScopeType.Event]);
    public static readonly MetricDefinition Headshots = new(0, decimal.MaxValue, FaultTolerance.Zero, "Headshots", [ScopeType.Player, ScopeType.Team, ScopeType.Event]);
    public static readonly MetricDefinition DamageDealt = new(0, decimal.MaxValue, FaultTolerance.Zero, "Damage Dealt", [ScopeType.Player, ScopeType.Team, ScopeType.Event]);

    /// <summary>
    /// Number of rounds won.
    /// </summary>
    public static readonly MetricDefinition Rounds = new(0, decimal.MaxValue, FaultTolerance.Zero, "Rounds", [ScopeType.Team, ScopeType.Event]);

    /// <summary>
    /// Number of maps won.
    /// </summary>
    public static readonly MetricDefinition Maps = new(0, decimal.MaxValue, FaultTolerance.Zero, "Maps", [ScopeType.Team, ScopeType.Event]);

    public static readonly MetricDefinition[] AllMetrics =
    [
        Kills,
        Deaths,
        Assists,
        Headshots,
        DamageDealt,
        Rounds,
        Maps
    ];
}