using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record ConstantMetricExpression<T>(T Value) : MetricExpression<T>
{
    public override T Evaluate(EventMetrics metrics) => Value;
}
