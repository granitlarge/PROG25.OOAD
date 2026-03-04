using PROG25.OOAD.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.Domain.ValueObjects.Soccer;

public record SoccerMatchState
(
    SoccerMatchTeamState HomeTeamState,
    SoccerMatchTeamState AwayTeamState
) : MatchState()
{
    public override decimal ResolveMetric(MetricType type)
    {
        return type switch
        {
            MetricType.Goals => GetTeamGoals(HomeTeamState.TeamId) + GetTeamGoals(AwayTeamState.TeamId),
            _ => throw new NotSupportedException($"Unsupported metric type: {type}"),
        };
    }

    public override decimal ResolvePlayerMetric(PlayerId playerId, MetricType type)
    {
        return base.ResolvePlayerMetric(playerId, type);
    }

    public override decimal ResolveTeamMetric(TeamId teamId, MetricType type)
    {
        return type switch
        {
            MetricType.Goals => GetTeamGoals(teamId),
            _ => throw new NotSupportedException($"Unsupported metric type: {type}"),
        };
    }

    private decimal GetTeamGoals(TeamId teamId)
    {
        return teamId == HomeTeamState.TeamId ? HomeTeamState.Score.Value : AwayTeamState.Score.Value;
    }
}