using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;

public record DimensionLessMetricExpression : MetricExpression<decimal>
{
    public DimensionLessMetricExpression(MetricDefinition definition)
    {
        Definition = definition;
    }

    public MetricDefinition Definition { get; }

    public override decimal Evaluate(EventMetrics metrics)
    {
        var relevantMetricValues = metrics.GetByDefinition(Definition);
        return relevantMetricValues.Single().Value;
    }
}