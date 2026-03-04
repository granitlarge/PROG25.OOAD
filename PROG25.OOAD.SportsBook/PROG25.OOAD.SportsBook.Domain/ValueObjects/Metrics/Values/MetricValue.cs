using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Values;

public record MetricValue
{
    public MetricValue(ScopedMetricDefinition definition, decimal value)
    {
        if (!definition.Metric.IsValidMetricValue(value))
        {
            throw new ArgumentException("Value is not valid for the given metric definition.", nameof(value));
        }

        Definition = definition;
        Value = value;
    }

    public ScopedMetricDefinition Definition { get; }
    public decimal Value { get; }
}