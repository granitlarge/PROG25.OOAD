namespace PROG25.OOAD.Domain.ValueObjects.Metrics;

public record TotalsMetric : Metric
{
    public MetricType Type { get; }
    public TotalsMetric(MetricType type)
    {
        Type = type;
    }

    public override decimal Resolve(MatchState matchStatistics)
    {
        return Type switch
        {
            MetricType.Goals => matchStatistics.GetTotalGoals(),
            MetricType.Corners => matchStatistics.GetTotalCorners(),
            _ => throw new InvalidOperationException("Unsupported metric type."),
        };
    }
}