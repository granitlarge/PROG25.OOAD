using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

public record BooleanMetricExpressionTimestamp : EventDataTimestamp
{
    public BooleanMetricExpressionTimestamp
    (
        MetricExpression<bool> metricExpression,
        bool expectedValue
    )
        : base(EventDataTimestampType.EventData)
    {
        MetricExpression = metricExpression;
        ExpectedValue = expectedValue;
    }

    public MetricExpression<bool> MetricExpression { get; }
    public bool ExpectedValue { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var value = MetricExpression.Evaluate(currentEventData.Metrics);
        return value == ExpectedValue;
    }
}