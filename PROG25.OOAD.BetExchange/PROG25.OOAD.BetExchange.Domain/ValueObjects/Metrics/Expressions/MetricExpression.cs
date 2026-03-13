using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public abstract record MetricExpression<T>
{
    public abstract T Evaluate(EventMetrics metrics);
}
