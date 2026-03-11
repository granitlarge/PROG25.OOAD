using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

public record MetricValue
{
    public MetricValue(Scope scope, MetricDefinition metric, decimal value)
    {
        if (!metric.IsValidMetricValue(value) || !metric.IsValidScope(scope))
        {
            throw new ArgumentException("Value or scope is not valid for the given metric definition.", nameof(value));
        }
        Scope = scope;
        Metric = metric;
        Value = value;
    }

    public Scope Scope { get; }
    public MetricDefinition Metric { get; }
    public decimal Value { get; }
}