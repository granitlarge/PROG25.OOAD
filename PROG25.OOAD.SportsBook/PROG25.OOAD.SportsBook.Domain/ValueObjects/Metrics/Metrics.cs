using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class Metrics
{
    public static (Scope Scope, MetricDefinition MetricDefinition)[] GetPlayerScopedMetrics(MetricDefinition[] allMetrics, PlayerId playerId)
    {
        var scope = new PlayerScope(playerId);
        return [.. allMetrics.Select(metric => metric.ForScope(scope))];
    }

    public static (Scope Scope, MetricDefinition MetricDefinition)[] GetTeamScopedMetrics(MetricDefinition[] allMetrics, TeamId teamId)
    {
        var scope = new TeamScope(teamId);
        return [.. allMetrics.Select(metric => metric.ForScope(scope))];
    }
}