using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

public record MetricValue
{
    public MetricValue
    (
        DimensionValue dimension,
        MetricDefinition definition,
        decimal value
    )
    {
        if (!definition.IsValidMetricValue(value))
        {
            throw new ArgumentException($"{nameof(value)} is an invalid metric value for metric {definition.Name}");
        }

        if (!definition.IsValidDimensionValue(dimension))
        {
            throw new ArgumentException("Invalid dimension value given metric definition");
        }

        Dimension = dimension;
        Definition = definition;
        Value = value;
    }

    public DimensionValue Dimension { get; }
    public MetricDefinition Definition { get; }
    public decimal Value { get; }
}