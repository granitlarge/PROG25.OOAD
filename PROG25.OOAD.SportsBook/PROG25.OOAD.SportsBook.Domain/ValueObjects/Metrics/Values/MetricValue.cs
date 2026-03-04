using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Values;

public record MetricValue
{
    public MetricValue(Scope scope, MetricDefinition metric, decimal value)
    {
        if (!metric.IsValidMetricValue(value))
        {
            throw new ArgumentException("Value is not valid for the given metric definition.", nameof(value));
        }
        Scope = scope;
        Metric = metric;
        Value = value;
    }

    public Scope Scope { get; }
    public MetricDefinition Metric { get; }
    public decimal Value { get; }
}