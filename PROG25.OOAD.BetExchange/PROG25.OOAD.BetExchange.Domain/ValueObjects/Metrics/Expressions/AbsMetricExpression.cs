using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record AbsMetricExpression(MetricExpression<decimal> Source) : MetricExpression<decimal>
{
    public override decimal Evaluate(EventMetrics metrics)
    {
        return Math.Abs(Source.Evaluate(metrics));
    }
}