using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;

/// <summary>
/// Represents a metric.
/// </summary>
public record MetricDefinition
{
    public MetricDefinition(decimal minValue, decimal maxValue, FaultTolerance faultTolerance, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Metric type name cannot be null or whitespace.", nameof(name));
        }

        if (minValue > maxValue)
        {
            throw new ArgumentException("Minimum value cannot be greater than maximum value.");
        }

        Name = name;
        MinValue = minValue;
        MaxValue = maxValue;
        FaultTolerance = faultTolerance;
    }

    public string Name { get; }
    public decimal MinValue { get; }
    public decimal MaxValue { get; }
    private FaultTolerance FaultTolerance { get; }

    public bool IsValidMetricValue(decimal value)
    {
        var isWithinRange = value >= MinValue && value <= MaxValue;
        return isWithinRange;
    }

    public ComparisonResult Compare(decimal firstValue, decimal secondValue)
    {
        if (!IsValidMetricValue(firstValue) || !IsValidMetricValue(secondValue))
        {
            throw new ArgumentException("One or both metric values are invalid for this metric.");
        }

        return FaultTolerance.Compare(firstValue, secondValue);
    }
}

public record ScopedMetricDefinition(Scope Scope, MetricDefinition Metric);