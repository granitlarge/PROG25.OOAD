using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class SoccerMetrics
{
    public static readonly MetricDefinition Goals = new(0, decimal.MaxValue, FaultTolerance.Zero, "Goals");
    public static readonly MetricDefinition Assists = new(0, decimal.MaxValue, FaultTolerance.Zero, "Assists");
    public static readonly MetricDefinition YellowCards = new(0, decimal.MaxValue, FaultTolerance.Zero, "Yellow Cards");
    public static readonly MetricDefinition RedCards = new(0, decimal.MaxValue, FaultTolerance.Zero, "Red Cards");
    public static readonly MetricDefinition Corners = new(0, decimal.MaxValue, FaultTolerance.Zero, "Corners");

    public static readonly ScopedMetricDefinition[] EventScopedMetrics = new[]
    {
        Goals,
        Assists,
        YellowCards,
        RedCards,
        Corners
    }.Select(metric => new ScopedMetricDefinition(EventScope.Instance, metric)).ToArray();

    public static ScopedMetricDefinition[] GetPlayerScopedMetrics(PlayerId playerId)
    {
        var scope = new PlayerScope(playerId);
        return new[]
        {
            Goals,
            Assists,
            YellowCards,
            RedCards,
            Corners
        }.Select(metric => new ScopedMetricDefinition(scope, metric)).ToArray();
    }

    public static ScopedMetricDefinition[] GetTeamScopedMetrics(TeamId teamId)
    {
        var scope = new TeamScope(teamId);
        return new[]
        {
            Goals,
            Assists,
            YellowCards,
            RedCards,
            Corners
        }.Select(metric => new ScopedMetricDefinition(scope, metric)).ToArray();
    }
}