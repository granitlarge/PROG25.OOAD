using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Timestamps.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations;

public record ComparisonEventMetricMarketConfiguration : EventMarketConfiguration
{
    public ComparisonEventMetricMarketConfiguration
    (
        ComparisonMetricExpression expression,
        EventDataTimestamp timestamp,
        ComparisonResult expectedComparisonResult,
        string name
    ) : base(timestamp, name)
    {
        ExpectedComparisonResult = expectedComparisonResult;
        Expression = expression;
    }

    public decimal ReferenceValue { get; }
    public ComparisonResult ExpectedComparisonResult { get; }
    public ComparisonMetricExpression Expression { get; }
}