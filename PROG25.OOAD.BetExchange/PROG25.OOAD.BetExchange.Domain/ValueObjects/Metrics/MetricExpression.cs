using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics;

public abstract record MetricExpression<T>
{
    public abstract T Evaluate(EventMetrics metrics);
}

public record ConstantMetricExpression<T>(T Value) : MetricExpression<T>
{
    public override T Evaluate(EventMetrics metrics) => Value;
}

public record FilteredAndAggregatedMetricExpression : MetricExpression<decimal>
{
    public FilteredAndAggregatedMetricExpression(MetricDefinition definition, ImmutableHashSet<DimensionFilter> filters)
    {
        foreach (var filter in filters)
        {
            if (!definition.IsValidDimensionFilter(filter))
            {
                throw new ArgumentException($"Dimension filter '{filter}' is not valid for metric '{definition.Name}'.", nameof(filters));
            }
        }

        Definition = definition;
        Filters = filters;
    }

    public MetricDefinition Definition { get; }
    public ImmutableHashSet<DimensionFilter> Filters { get; }

    public override decimal Evaluate(EventMetrics metrics)
    {
        var relevantMetricValues = metrics.GetByDefinition(Definition);
        return Definition.Aggregate(Definition.Filter(Filters, relevantMetricValues));
    }
}

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

public record ComparisonMetricExpression(MetricExpression<decimal> Left, MetricExpression<decimal> Right, FaultTolerance FaultTolerance) : MetricExpression<ComparisonResult>
{
    public override ComparisonResult Evaluate(EventMetrics metrics)
    {
        var left = Left.Evaluate(metrics);
        var right = Right.Evaluate(metrics);
        return FaultTolerance.Compare(left, right);
    }
}

public enum ArithmeticOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}