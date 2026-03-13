using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record OrMetricExpression(MetricExpression<bool> Left, MetricExpression<bool> Right) : MetricExpression<bool>
{
    public override bool Evaluate(EventMetrics metrics)
    {
        var left = Left.Evaluate(metrics);
        var right = Right.Evaluate(metrics);
        return left || right;
    }
}
