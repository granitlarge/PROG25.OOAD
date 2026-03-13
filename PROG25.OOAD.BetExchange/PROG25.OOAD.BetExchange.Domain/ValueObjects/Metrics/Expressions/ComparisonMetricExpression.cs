using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record ComparisonMetricExpression(MetricExpression<decimal> Left, MetricExpression<decimal> Right, FaultTolerance FaultTolerance) : MetricExpression<ComparisonResult>
{
    public override ComparisonResult Evaluate(EventMetrics metrics)
    {
        var left = Left.Evaluate(metrics);
        var right = Right.Evaluate(metrics);
        return FaultTolerance.Compare(left, right);
    }
}

public record ComparisonBooleanMetricExpression
(
    MetricExpression<decimal> Left,
    MetricExpression<decimal> Right,
    FaultTolerance FaultTolerance,
    ComparisonResult ExpectedComparisonResult
) : MetricExpression<bool>
{
    private readonly ComparisonMetricExpression _comparisonMetricExpression = new(Left, Right, FaultTolerance);
    public override bool Evaluate(EventMetrics metrics)
    {
        return _comparisonMetricExpression.Evaluate(metrics) == ExpectedComparisonResult;
    }
}