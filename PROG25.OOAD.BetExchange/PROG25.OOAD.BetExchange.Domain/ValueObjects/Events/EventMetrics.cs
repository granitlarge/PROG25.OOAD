using System.Collections.Immutable;
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

    public ImmutableHashSet<MetricValue> GetByDefinition(MetricDefinition definition)
    {
        return [.. _metricValues.Where(mv => mv.Definition == definition)];
    }
}