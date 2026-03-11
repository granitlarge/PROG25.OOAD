using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;

/// <summary>
/// Represents a metric.
/// </summary>
public record MetricDefinition
{
    public MetricDefinition
    (
        decimal minValue,
        decimal maxValue,
        ScopeType scopeType,
        AggregationType crossScopeAggregationType,
        FaultTolerance faultTolerance,
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Metric type name cannot be null or whitespace.", nameof(name));
        }

        MinValue = minValue;
        MaxValueValue = maxValue;
        FaultTolerance = faultTolerance;
        Name = name;
        ScopeType = scopeType;
        CrossScopeAggregationType = crossScopeAggregationType;
    }

    private decimal MinValue { get; }
    private decimal MaxValueValue { get; }

    private FaultTolerance FaultTolerance { get; }
    public ScopeType ScopeType { get; }
    private AggregationType CrossScopeAggregationType { get; }

    public string Name { get; }

    public bool IsValidMetricValue(decimal value)
    {
        var isWithinBounds = value >= MinValue && value <= MaxValueValue;
        return isWithinBounds;
    }

    public bool IsValidScope(Scope scope)
    {
        return ScopeType == scope.Type;
    }

    public bool IsValidScopeType(ScopeType scope)
    {
        return scope == ScopeType;
    }

    public MetricValue Aggregate(Scope requestedAggregationScope, ImmutableHashSet<MetricValue> metricValues)
    {
        return Aggregate(requestedAggregationScope.Type, metricValues).Single(mv => mv.Scope == requestedAggregationScope);
    }

    public ImmutableHashSet<MetricValue> Aggregate(ScopeType requestedAggregationLevel, ImmutableHashSet<MetricValue> metricValues)
    {
        var allMetricValuesBelongToThisDefinition = metricValues.All(mv => mv.Metric == this);
        if (!allMetricValuesBelongToThisDefinition)
        {
            throw new ArgumentException("Cannot aggregate metric values of other definitions");
        }

        if (requestedAggregationLevel == ScopeType)
        {
            return [metricValues.Single(mv => mv.Metric == this)];
        }

        if (CrossScopeAggregationType == AggregationType.None)
        {
            throw new InvalidOperationException("Impossible to aggregate");
        }

        var isValidAggregation = requestedAggregationLevel switch
        {
            ScopeType.Player => ScopeType == ScopeType.Player,
            ScopeType.Team => ScopeType == ScopeType.Team || ScopeType == ScopeType.Player,
            ScopeType.Event => ScopeType == ScopeType.Team || ScopeType == ScopeType.Player || ScopeType == ScopeType.Event,
            _ => throw new NotImplementedException()
        };

        if (!isValidAggregation)
        {
            throw new InvalidOperationException();
        }

        decimal aggregationFunction(IEnumerable<MetricValue> values) => CrossScopeAggregationType switch
        {
            AggregationType.Sum => values.Sum(v => v.Value),
            _ => throw new NotImplementedException()
        };

        return requestedAggregationLevel switch
        {
            ScopeType.Event => [new MetricValue(EventScope.Instance, this, aggregationFunction(metricValues))],
            ScopeType.Team => [
                ..
                metricValues
                .GroupBy(rm => ((PlayerScope)rm.Scope).TeamId)
                .Select(g => new MetricValue(new TeamScope(g.Key), this, aggregationFunction(g)))
            ],
            _ => throw new NotImplementedException()
        };

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

public enum AggregationType
{
    None,
    Sum,
}