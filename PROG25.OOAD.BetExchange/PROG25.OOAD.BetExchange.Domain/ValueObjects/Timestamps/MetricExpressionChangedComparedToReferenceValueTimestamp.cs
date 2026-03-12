using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps;

public record MetricExpressionChangedComparedToReferenceValueTimestamp : EventDataTimestamp
{
    public MetricExpressionChangedComparedToReferenceValueTimestamp
    (
        MetricExpression<decimal> metricExpression,
        decimal referenceValue,
        ComparisonResult comparisonResult,
        FaultTolerance faultTolerance
    )
        : base(EventDataTimestampType.EventData)
    {
        if (comparisonResult == ComparisonResult.Equal)
        {
            throw new ArgumentException("Comparison result cannot be Equal for a threshold comparison.", nameof(comparisonResult));
        }

        MetricExpression = metricExpression;
        ExpectedComparisonResult = comparisonResult;
        Threshold = referenceValue;
        FaultTolerance = faultTolerance;
    }

    public FaultTolerance FaultTolerance { get; }
    public MetricExpression<decimal> MetricExpression { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public decimal Threshold { get; }

    public override bool HasOccurred(EventData currentEventData)
    {
        var value = MetricExpression.Evaluate(currentEventData.Metrics);
        return IsExceeded(value);
    }

    private bool IsExceeded(decimal metricValue)
    {
        var result = FaultTolerance.Compare(metricValue, Threshold);
        return result != ComparisonResult.Equal && ExpectedComparisonResult == result;
    }
}