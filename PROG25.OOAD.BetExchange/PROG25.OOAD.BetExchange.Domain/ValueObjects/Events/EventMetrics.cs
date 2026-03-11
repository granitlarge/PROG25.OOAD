using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

namespace PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;

public record EventMetrics
{
    private readonly ISet<MetricValue> _metricValues;

    public EventMetrics(ISet<MetricValue> metricValues)
    {
        _metricValues = metricValues;
    }

    public decimal Extract(ImmutableHashSet<DimensionFilter> filters, MetricDefinition definition)
    {
        if (!IsSupportedMetric(definition))
        {
            throw new InvalidOperationException("Unsupported metric");
        }
#warning is a sum really ALWAYS the right option here? Shouldn't the metric definition know how to aggregate itself?
        return definition.Filter(filters, [.. _metricValues.Where(mv => mv.Definition == definition)]).Sum(mv => mv.Value);
    }

    public ImmutableHashSet<(DimensionFilter Query, decimal Value)> ExtractAll(List<string> dimensionNames, MetricDefinition definition)
    {
        if (!IsSupportedMetric(definition))
        {
            throw new InvalidOperationException("Unsupported metric");
        }

        return
                [.. _metricValues
                    .Where(metricValue => metricValue.Definition == definition)
                    .Select(mv => (MetricValue: mv, DimensionValue: mv.Dimension.Value.Where(kv => dimensionNames.Any(dn => kv.Key == dn)).OrderBy(kv => kv.Key).Select(kv => kv.Value).ToList()))
#warning Does this grouping really work?
                    .GroupBy(metricValueAndDimensionValue => string.Join("#", metricValueAndDimensionValue.DimensionValue))
                    .Select
                    (
                        metricValueAndDimensionValue =>
                        (
                            new DimensionFilter
                            (
                                dimensionNames
                                .OrderBy(dn => dn)
                                .Select((value, index) => new KeyValuePair<string, object>(dimensionNames[index], metricValueAndDimensionValue.First().DimensionValue[index]))
                                .ToDictionary(kv => kv.Key, kv => kv.Value)
                                .ToImmutableDictionary(),
                                definition.Dimension
                            ),
                            metricValueAndDimensionValue.Sum(x => x.MetricValue.Value)
                        )
                    )];
    }

    public bool IsSupportedMetric(MetricDefinition metric)
    {
        return _metricValues.Any(mv => mv.Definition == metric);
    }

}