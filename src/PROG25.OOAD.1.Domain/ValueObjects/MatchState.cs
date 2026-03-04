using PROG25.OOAD.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.Domain.ValueObjects;

public abstract record MatchState
(

)
{
    public virtual decimal ResolveTeamMetric(TeamId teamId, MetricType type)
    {
        throw new NotImplementedException();
    }

    public virtual decimal ResolvePlayerMetric(PlayerId playerId, MetricType type)
    {
        throw new NotImplementedException();
    }

    public virtual decimal ResolveMetric(MetricType type)
    {
        throw new NotImplementedException();
    }
}