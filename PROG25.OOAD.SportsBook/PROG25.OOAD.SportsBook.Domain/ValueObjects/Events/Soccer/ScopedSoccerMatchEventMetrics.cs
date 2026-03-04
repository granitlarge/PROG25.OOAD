using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events.Soccer;

public record ScopedSoccerMatchEventMetrics : ScopedEventMetrics
{
    public ScopedSoccerMatchEventMetrics
    (
        object scopeIdentifier,
        TimeSpan elapsedMatchTime,
        TimeSpan elapsedActualTime,
        uint score,
        uint yellowCards,
        uint redCards,
        uint corners
    ) : base(new ScopedEventStateId(scopeIdentifier))
    {
        Score = score;
        YellowCards = yellowCards;
        RedCards = redCards;
        Corners = corners;
        ElapsedMatchTime = elapsedMatchTime;
        ElapsedActualTime = elapsedActualTime;
    }

    public uint Score { get; private set; }
    public uint YellowCards { get; private set; }
    public uint RedCards { get; private set; }
    public uint Corners { get; private set; }
    public TimeSpan ElapsedMatchTime { get; private set; }
    public TimeSpan ElapsedActualTime { get; private set; }

    public override decimal Extract(MetricType metricType)
    {
        return metricType switch
        {
            MetricType.Points => Score,
            MetricType.YellowCards => YellowCards,
            MetricType.RedCards => RedCards,
            MetricType.Corners => Corners,
            MetricType.ElapsedMatchTimeSeconds => (decimal)ElapsedMatchTime.TotalSeconds,
            MetricType.ElapsedActualTimeSeconds => (decimal)ElapsedActualTime.TotalSeconds,
            _ => throw new NotSupportedException($"Metric type {metricType} is not supported in ScopedSoccerMatchEventMetrics."),
        };
    }
}