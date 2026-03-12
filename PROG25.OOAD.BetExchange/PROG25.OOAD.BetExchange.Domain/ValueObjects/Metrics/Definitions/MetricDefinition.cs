using System.Collections.Immutable;
using System.Data;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Utility;

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
        FaultTolerance faultTolerance,
        string name,
        DimensionDefinition dimension,
        Aggregation aggregation
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
        Dimension = dimension;
        Aggregation = aggregation;
    }

    private decimal MinValue { get; }

    private decimal MaxValueValue { get; }

    private FaultTolerance FaultTolerance { get; }

    private Aggregation Aggregation { get; }

    public DimensionDefinition Dimension { get; }

    public string Name { get; }

    public bool IsValidMetricValue(decimal value)
    {
        var isWithinBounds = value >= MinValue && value <= MaxValueValue;
        return isWithinBounds;
    }

    public decimal Aggregate(ImmutableHashSet<MetricValue> metricValues)
    {
        if (metricValues.Any(mv => mv.Definition != this))
        {
            throw new ArgumentException("This metric definition cannot be used to aggregate metric values of other metric definitions.");
        }

        return Aggregation switch
        {
            Aggregation.None => throw new InvalidOperationException("This metric definition cannot be aggregated"),
            Aggregation.Sum => metricValues.Sum(mv => mv.Value),
            _ => throw new NotImplementedException($"{Enum.GetName(Aggregation)!}"),
        };
    }

    /// <summary>
    /// Filters  the given metric values based on the given dimension filters. 
    /// A metric value is considered relevant for a dimension filter if it matches ALL criteria specified in the dimension filter. 
    /// If multiple dimension filters are given, a metric value is considered relevant if it matches ANY of the dimension filters.
    /// </summary>
    public ImmutableHashSet<MetricValue> Filter(ImmutableHashSet<DimensionFilter> filters, ImmutableHashSet<MetricValue> metricValues)
    {
        if (metricValues.Any(mv => mv.Definition != this))
        {
            throw new ArgumentException($"This metric definition cannot be used to filter metric values of other metric definitions.");
        }

        if (filters.Any(f => f.Definition != Dimension))
        {
            throw new ArgumentException($"A dimension query was specified that is invalid for this metric definition");
        }

        var hashSet = new HashSet<MetricValue>();
        if (filters.Count == 0)
        {
            foreach (var mv in metricValues)
                hashSet.Add(mv);
        }
        else
        {
            foreach (var metricValue in metricValues)
            {
                var isRelevantMetric = filters.Any(filter => filter.Value.All(dqv => metricValue.Dimension.Value.Contains(dqv)));
                if (isRelevantMetric)
                {
                    hashSet.Add(metricValue);
                }
            }
        }
        return [.. hashSet];
    }

    public ImmutableHashSet<(DimensionFilter Key, ImmutableHashSet<MetricValue> Values)> GroupBy(ImmutableHashSet<string> dimensionNames, ImmutableHashSet<MetricValue> metricValues)
    {
        if (metricValues.Any(mv => mv.Definition != this))
        {
            throw new ArgumentException($"This metric definition cannot be used to group metric values of other metric definitions.");
        }

        if (dimensionNames.Count == 0)
        {
            return [(new DimensionFilter(ImmutableDictionary<string, object>.Empty, Dimension), metricValues)];
        }

        return [.. metricValues
            .GroupBy(mv => new DynamicGroupingKey(dimensionNames.ToImmutableDictionary(dn => dn, dn => mv.Dimension.Value[dn])))
            .Select(group => (new DimensionFilter(group.Key.Values, Dimension), group.ToImmutableHashSet()))];
    }

    public ComparisonResult Compare(decimal firstValue, decimal secondValue)
    {
        if (!IsValidMetricValue(firstValue) || !IsValidMetricValue(secondValue))
        {
            throw new ArgumentException("One or both metric values are invalid for this metric.");
        }

        return FaultTolerance.Compare(firstValue, secondValue);
    }

    internal bool IsValidDimensionValue(DimensionValue dv)
    {
        return Dimension == dv.Definition;
    }

    internal bool IsValidDimensionFilter(DimensionFilter dq)
    {
        return dq.Definition == Dimension;
    }
}

public enum Aggregation
{
    None,
    Sum
}