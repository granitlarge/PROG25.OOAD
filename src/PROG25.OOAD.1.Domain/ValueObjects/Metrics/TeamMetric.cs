namespace PROG25.OOAD.Domain.ValueObjects.Metrics;

public record TeamMetric(TeamId TeamId, MetricType Type) : Metric
{
    public TeamId TeamId { get; } = TeamId;
    public MetricType Type { get; } = Type;

    public override decimal Resolve(MatchState matchStatistics)
    {
        return matchStatistics.ResolveTeamMetric(TeamId, Type);
    }
}