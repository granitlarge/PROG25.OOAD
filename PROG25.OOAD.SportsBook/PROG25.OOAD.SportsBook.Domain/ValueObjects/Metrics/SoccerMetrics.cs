using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class SoccerMetrics
{
    public static readonly MetricDefinition Goals = new(0, decimal.MaxValue, FaultTolerance.Zero, "Goals", [ScopeType.Player, ScopeType.Player, ScopeType.Event]);
    public static readonly MetricDefinition Assists = new(0, decimal.MaxValue, FaultTolerance.Zero, "Assists", [ScopeType.Player, ScopeType.Player, ScopeType.Event]);
    public static readonly MetricDefinition YellowCards = new(0, decimal.MaxValue, FaultTolerance.Zero, "Yellow Cards", [ScopeType.Player, ScopeType.Player, ScopeType.Event]);
    public static readonly MetricDefinition RedCards = new(0, decimal.MaxValue, FaultTolerance.Zero, "Red Cards", [ScopeType.Player, ScopeType.Player, ScopeType.Event]);
    public static readonly MetricDefinition Corners = new(0, decimal.MaxValue, FaultTolerance.Zero, "Corners", [ScopeType.Player, ScopeType.Player, ScopeType.Event]);

    public static readonly MetricDefinition[] AllMetrics =
    [
        Goals,
        Assists,
        YellowCards,
        RedCards,
        Corners
    ];
}