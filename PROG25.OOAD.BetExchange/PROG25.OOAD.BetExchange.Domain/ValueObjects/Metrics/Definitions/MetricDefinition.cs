using System.Collections.Immutable;
using System.Data;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

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
        DimensionDefinition dimension
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
    }

    private decimal MinValue { get; }

    private decimal MaxValueValue { get; }

    private FaultTolerance FaultTolerance { get; }

    public DimensionDefinition Dimension { get; }

    public string Name { get; }

    public bool IsValidMetricValue(decimal value)
    {
        var isWithinBounds = value >= MinValue && value <= MaxValueValue;
        return isWithinBounds;
    }

    /// <summary>
    /// Filters  the given metric values based on the given dimension queries. 
    /// A metric value is considered relevant for a dimension query if it matches ALL criteria specified in the dimension query. 
    /// If multiple dimension queries are given, a metric value is considered relevant if it matches ANY of the dimension queries.
    /// </summary>
    public IEnumerable<MetricValue> Filter(ImmutableHashSet<DimensionFilter> dimensionQueries, ImmutableHashSet<MetricValue> metricValues)
    {
        if (metricValues.Any(mv => mv.Definition != this))
        {
            throw new ArgumentException($"This metric definition cannot be used to filter metric values of other metric definition.");
        }

        if (dimensionQueries.Any(dq => dq.Definition != Dimension))
        {
            throw new ArgumentException($"A dimension query was specified that is invalid for this metric definition");
        }

        if (dimensionQueries.Count == 0)
        {
            foreach (var mv in metricValues)
                yield return mv;
        }
        else
        {
            foreach (var metricValue in metricValues)
            {
                var relevantDimensionQueries = dimensionQueries.Where(dq => metricValue.Dimension.Definition == dq.Definition).ToList();
                if (relevantDimensionQueries.Count == 0)
                    continue;

                var relevantDimensionQueriesGroupedByType = relevantDimensionQueries.GroupBy(rdq => rdq.Definition);
                foreach (var group in relevantDimensionQueriesGroupedByType)
                {
                    // If the metric agrees with ANY of the dimension filters, yield it
                    var isRelevantMetric = group.Any(dimensionQuery => dimensionQuery.Value.All(dqv => metricValue.Dimension.Value.Contains(dqv)));
                    if (isRelevantMetric)
                    {
                        yield return metricValue;
                    }
                }
            }
        }
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

    internal bool IsValidDimensionQuery(DimensionFilter dq)
    {
        return dq.Definition == Dimension;
    }
}