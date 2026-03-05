using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;

/// <summary>
/// Represents a metric.
/// </summary>
public record MetricDefinition
{
    public MetricDefinition
    (
        decimal minValue,
        decimal maxValue,
        FaultTolerance faultTolerance,
        string name,
        ImmutableHashSet<ScopeType> validScopes
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Metric type name cannot be null or whitespace.", nameof(name));
        }

        if (minValue > maxValue)
        {
            throw new ArgumentException("Minimum value cannot be greater than maximum value.");
        }

        if (validScopes.Count == 0)
        {
            throw new ArgumentException("ValidScopes must contain at least 1 value", nameof(validScopes));
        }

        Name = name;
        MinValue = minValue;
        MaxValue = maxValue;
        FaultTolerance = faultTolerance;
        ValidScopes = validScopes;
    }

    private ImmutableHashSet<ScopeType> ValidScopes { get; }
    private FaultTolerance FaultTolerance { get; }

    public string Name { get; }
    public decimal MinValue { get; }
    public decimal MaxValue { get; }

    public bool IsValidMetricValue(decimal value)
    {
        var isWithinRange = value >= MinValue && value <= MaxValue;
        return isWithinRange;
    }

    public bool IsValidScopeType(ScopeType scope)
    {
        return ValidScopes.Contains(scope);
    }

    public bool IsValidScope(Scope scope)
    {
        return ValidScopes.Contains(scope.Type);
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