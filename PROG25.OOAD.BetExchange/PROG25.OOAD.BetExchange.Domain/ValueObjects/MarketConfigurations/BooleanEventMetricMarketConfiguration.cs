using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record BooleanEventMetricMarketConfiguration : EventMarketConfiguration
{
    public BooleanEventMetricMarketConfiguration
    (
        MetricExpression<bool> expression,
        bool expectedExpressionResult,
        EventDataTimestamp timestamp,
        string name
    ) : base(timestamp, name)
    {
        ExpectedExpressionResult = expectedExpressionResult;
        Expression = expression;
    }

    public bool ExpectedExpressionResult { get; }
    public MetricExpression<bool> Expression { get; }
}