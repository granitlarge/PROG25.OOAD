using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record ArithmeticMetricExpression(ArithmeticOperation Op, MetricExpression<decimal> Left, MetricExpression<decimal> Right) : MetricExpression<decimal>
{
    public override decimal Evaluate(EventMetrics metrics)
    {
        var left = Left.Evaluate(metrics);
        var right = Right.Evaluate(metrics);

        return Op switch
        {
            ArithmeticOperation.Addition => left + right,
            ArithmeticOperation.Subtraction => left - right,
            ArithmeticOperation.Multiplication => left * right,
            ArithmeticOperation.Division => left / right,
            _ => throw new NotImplementedException()
        };
    }
}
public enum ArithmeticOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}