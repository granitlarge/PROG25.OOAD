using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class CS2Metrics
{
    public static readonly MetricDefinition Kills = new(0, decimal.MaxValue, FaultTolerance.Zero, "Kills");
    public static readonly MetricDefinition Deaths = new(0, decimal.MaxValue, FaultTolerance.Zero, "Deaths");
    public static readonly MetricDefinition Assists = new(0, decimal.MaxValue, FaultTolerance.Zero, "Assists");
    public static readonly MetricDefinition Headshots = new(0, decimal.MaxValue, FaultTolerance.Zero, "Headshots");
    public static readonly MetricDefinition DamageDealt = new(0, decimal.MaxValue, FaultTolerance.Zero, "Damage Dealt");
    public static readonly MetricDefinition Rounds = new(0, decimal.MaxValue, FaultTolerance.Zero, "Rounds");
}